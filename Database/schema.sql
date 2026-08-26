-- ============================================
-- GapsCRM — Schema DB (fonte di verita', Database First)
-- SQL Server. Creazione tabelle + seed dati fake.
-- ============================================

-- ============================================
-- 1. TABELLE SENZA DIPENDENZE
-- ============================================

CREATE TABLE Agente (
    id_agente INT IDENTITY(1,1) PRIMARY KEY,
    nome NVARCHAR(50) NOT NULL,
    cognome NVARCHAR(50) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    telefono NVARCHAR(20),
    data_assunzione DATE NOT NULL,
    attivo BIT NOT NULL DEFAULT 1
);

CREATE TABLE AziendaCliente (
    id_azienda_cliente INT IDENTITY(1,1) PRIMARY KEY,
    ragione_sociale NVARCHAR(150) NOT NULL,
    partita_iva NVARCHAR(20) NOT NULL UNIQUE,
    codice_fiscale NVARCHAR(20),
    indirizzo NVARCHAR(150),
    citta NVARCHAR(50),
    cap NVARCHAR(10),
    provincia NVARCHAR(2),
    email NVARCHAR(100),
    telefono NVARCHAR(20),
    sito_web NVARCHAR(150),
    note NVARCHAR(MAX),
    attivo BIT NOT NULL DEFAULT 1
);

CREATE TABLE Prodotto (
    id_prodotto INT IDENTITY(1,1) PRIMARY KEY,
    nome NVARCHAR(100) NOT NULL,
    descrizione NVARCHAR(500),
    tipo NVARCHAR(20) NOT NULL CHECK (tipo IN ('Macchinario', 'Servizio')),
    codice NVARCHAR(30) NOT NULL UNIQUE,
    prezzo_listino DECIMAL(10,2) NOT NULL,
    attivo BIT NOT NULL DEFAULT 1
);

-- ============================================
-- 2. TABELLE CON UNA DIPENDENZA
-- ============================================

CREATE TABLE Contatto (
    id_contatto INT IDENTITY(1,1) PRIMARY KEY,
    id_azienda_cliente INT NOT NULL,
    nome NVARCHAR(50) NOT NULL,
    cognome NVARCHAR(50) NOT NULL,
    ruolo NVARCHAR(50),
    email NVARCHAR(100),
    telefono NVARCHAR(20),
    cellulare NVARCHAR(20),
    note NVARCHAR(MAX),
    attivo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Contatto_AziendaCliente FOREIGN KEY (id_azienda_cliente)
        REFERENCES AziendaCliente(id_azienda_cliente)
);

-- ============================================
-- 3. ORDINE (dipende da AziendaCliente, Agente, Contatto)
-- ============================================

CREATE TABLE Ordine (
    id_ordine INT IDENTITY(1,1) PRIMARY KEY,
    id_azienda_cliente INT NOT NULL,
    id_agente INT NOT NULL,
    id_contatto_riferimento INT NULL,
    data_ordine DATETIME2 NOT NULL DEFAULT GETDATE(),
    -- 'Annullato' aggiunto successivamente al set iniziale (Aperto/InTrattativa/Vinto/Perso):
    -- stato non reversibile una volta Vinto/Perso/Annullato, enforcement applicativo in OrdiniController
    stato NVARCHAR(20) NOT NULL CONSTRAINT CK_Ordine_Stato CHECK (stato IN ('Aperto', 'InTrattativa', 'Vinto', 'Perso', 'Annullato')),
    note NVARCHAR(MAX),
    CONSTRAINT FK_Ordine_AziendaCliente FOREIGN KEY (id_azienda_cliente)
        REFERENCES AziendaCliente(id_azienda_cliente),
    CONSTRAINT FK_Ordine_Agente FOREIGN KEY (id_agente)
        REFERENCES Agente(id_agente),
    CONSTRAINT FK_Ordine_Contatto FOREIGN KEY (id_contatto_riferimento)
        REFERENCES Contatto(id_contatto)
);

-- ============================================
-- 4. RIGAORDINE (dipende da Ordine, Prodotto)
-- ============================================

CREATE TABLE RigaOrdine (
    id_riga_ordine INT IDENTITY(1,1) PRIMARY KEY,
    id_ordine INT NOT NULL,
    id_prodotto INT NOT NULL,
    quantita INT NOT NULL CHECK (quantita > 0),
    prezzo_pattuito DECIMAL(10,2) NOT NULL,
    sconto DECIMAL(10,2) NOT NULL DEFAULT 0,
    totale_riga AS (quantita * prezzo_pattuito - sconto) PERSISTED,
    CONSTRAINT FK_RigaOrdine_Ordine FOREIGN KEY (id_ordine)
        REFERENCES Ordine(id_ordine),
    CONSTRAINT FK_RigaOrdine_Prodotto FOREIGN KEY (id_prodotto)
        REFERENCES Prodotto(id_prodotto)
);

-- ============================================
-- 5. LOGATTIVITA (dipende da Ordine, Contatto, Agente)
-- ============================================

CREATE TABLE LogAttivita (
    id_log_attivita INT IDENTITY(1,1) PRIMARY KEY,
    id_ordine INT NULL,
    id_contatto INT NULL,
    id_agente INT NOT NULL,
    data_ora DATETIME2 NOT NULL DEFAULT GETDATE(),
    tipo_attivita NVARCHAR(20) NOT NULL CHECK (tipo_attivita IN ('Chiamata', 'Email', 'Incontro', 'Note', 'Altro')),
    oggetto NVARCHAR(150),
    descrizione NVARCHAR(MAX),
    esito NVARCHAR(100),
    allegato_url NVARCHAR(300),
    CONSTRAINT FK_LogAttivita_Ordine FOREIGN KEY (id_ordine)
        REFERENCES Ordine(id_ordine),
    CONSTRAINT FK_LogAttivita_Contatto FOREIGN KEY (id_contatto)
        REFERENCES Contatto(id_contatto),
    CONSTRAINT FK_LogAttivita_Agente FOREIGN KEY (id_agente)
        REFERENCES Agente(id_agente)
);

-- ============================================
-- SEED DATI FITTIZI
-- ============================================

-- ============================================
-- 1. AGENTE (6 righe)
-- ============================================
INSERT INTO Agente (nome, cognome, email, telefono, data_assunzione, attivo) VALUES
('Marco', 'Bianchi', 'marco.bianchi@gapscrm.it', '3331234567', '2019-03-15', 1),
('Giulia', 'Ferrari', 'giulia.ferrari@gapscrm.it', '3357654321', '2020-06-01', 1),
('Luca', 'Colombo', 'luca.colombo@gapscrm.it', '3391122334', '2021-01-10', 1),
('Sara', 'Romano', 'sara.romano@gapscrm.it', '3405556677', '2018-09-20', 1),
('Davide', 'Ricci', 'davide.ricci@gapscrm.it', '3288889900', '2022-04-05', 1),
('Elena', 'Conti', 'elena.conti@gapscrm.it', '3311239876', '2023-02-14', 1);

-- ============================================
-- 2. AZIENDACLIENTE (15 righe)
-- ============================================
INSERT INTO AziendaCliente (ragione_sociale, partita_iva, codice_fiscale, indirizzo, citta, cap, provincia, email, telefono, sito_web, note, attivo) VALUES
('Meccanica Lombarda Srl', 'IT00000000001', '00000000001', 'Via dell''Industria 12', 'Milano', '20100', 'MI', 'info@meccanicalombarda.it', '0221234567', 'www.meccanicalombarda.it', NULL, 1),
('Officine Venete SpA', 'IT00000000002', '00000000002', 'Via Torino 45', 'Vicenza', '36100', 'VI', 'info@officinevenete.it', '0444123456', 'www.officinevenete.it', NULL, 1),
('Tornerie Piemontesi Srl', 'IT00000000003', '00000000003', 'Corso Francia 88', 'Torino', '10100', 'TO', 'info@tornerie-piemontesi.it', '0119876543', 'www.tornerie-piemontesi.it', NULL, 1),
('Industrie Metalmeccaniche Emiliane SpA', 'IT00000000004', '00000000004', 'Via Emilia 200', 'Modena', '41100', 'MO', 'info@ime-spa.it', '0591112233', 'www.ime-spa.it', NULL, 1),
('Fonderie Toscane Srl', 'IT00000000005', '00000000005', 'Via Pistoiese 33', 'Prato', '59100', 'PO', 'info@fonderietoscane.it', '0574445566', 'www.fonderietoscane.it', NULL, 1),
('Costruzioni Meccaniche Adriatiche Srl', 'IT00000000006', '00000000006', 'Via Marina 7', 'Ancona', '60100', 'AN', 'info@cma-adriatiche.it', '0712223344', 'www.cma-adriatiche.it', NULL, 1),
('Gruppo Siderurgico Bresciano SpA', 'IT00000000007', '00000000007', 'Via Brescia Sud 15', 'Brescia', '25100', 'BS', 'info@gsb-spa.it', '0303334455', 'www.gsb-spa.it', NULL, 1),
('Meccanica di Precisione Bergamasca Srl', 'IT00000000008', '00000000008', 'Via Bergamo 21', 'Bergamo', '24100', 'BG', 'info@mpb-srl.it', '0355556677', 'www.mpb-srl.it', NULL, 1),
('Officine Meccaniche Friulane Srl', 'IT00000000009', '00000000009', 'Via Udine 9', 'Udine', '33100', 'UD', 'info@omf-srl.it', '0432667788', 'www.omf-srl.it', NULL, 1),
('Tecnomeccanica Ligure Srl', 'IT00000000010', '00000000010', 'Via Genova 55', 'Genova', '16100', 'GE', 'info@tecnoligure.it', '0107778899', 'www.tecnoligure.it', NULL, 1),
('Automazioni Industriali Venete Srl', 'IT00000000011', '00000000011', 'Via Padova 3', 'Padova', '35100', 'PD', 'info@aiv-srl.it', '0498889900', 'www.aiv-srl.it', NULL, 1),
('Carpenteria Metallica Umbra Srl', 'IT00000000012', '00000000012', 'Via Perugia 18', 'Perugia', '06100', 'PG', 'info@cmu-srl.it', '0759990011', 'www.cmu-srl.it', NULL, 1),
('Meccanica Generale Pugliese Srl', 'IT00000000013', '00000000013', 'Via Bari 60', 'Bari', '70100', 'BA', 'info@mgp-srl.it', '0801001122', 'www.mgp-srl.it', NULL, 1),
('Fabbrica Macchine Trentina Srl', 'IT00000000014', '00000000014', 'Via Trento 27', 'Trento', '38100', 'TN', 'info@fmt-srl.it', '0461112233', 'www.fmt-srl.it', NULL, 1),
('Industria Meccanica Campana SpA', 'IT00000000015', '00000000015', 'Via Napoli 100', 'Napoli', '80100', 'NA', 'info@imc-spa.it', '0812223344', 'www.imc-spa.it', NULL, 1);

-- ============================================
-- 3. CONTATTO (30 righe, 2 per azienda)
-- ============================================
INSERT INTO Contatto (id_azienda_cliente, nome, cognome, ruolo, email, telefono, cellulare, note, attivo) VALUES
(1, 'Alberto', 'Rossi', 'Direttore Acquisti', 'a.rossi@meccanicalombarda.it', '0221234568', '3331112233', NULL, 1),
(1, 'Chiara', 'Villa', 'Responsabile Tecnico', 'c.villa@meccanicalombarda.it', '0221234569', '3331112234', NULL, 1),
(2, 'Paolo', 'Bertolini', 'Direttore Generale', 'p.bertolini@officinevenete.it', '0444123457', '3341112233', NULL, 1),
(2, 'Francesca', 'Marin', 'Ufficio Acquisti', 'f.marin@officinevenete.it', '0444123458', '3341112234', NULL, 1),
(3, 'Roberto', 'Gallo', 'Titolare', 'r.gallo@tornerie-piemontesi.it', '0119876544', '3351112233', NULL, 1),
(3, 'Silvia', 'Testa', 'Amministrazione', 's.testa@tornerie-piemontesi.it', '0119876545', '3351112234', NULL, 1),
(4, 'Massimo', 'Neri', 'Responsabile Produzione', 'm.neri@ime-spa.it', '0591112234', '3361112233', NULL, 1),
(4, 'Valentina', 'Costa', 'Ufficio Tecnico', 'v.costa@ime-spa.it', '0591112235', '3361112234', NULL, 1),
(5, 'Andrea', 'Ferri', 'Direttore Acquisti', 'a.ferri@fonderietoscane.it', '0574445567', '3371112233', NULL, 1),
(5, 'Laura', 'Pieraccini', 'Responsabile Qualita', 'l.pieraccini@fonderietoscane.it', '0574445568', '3371112234', NULL, 1),
(6, 'Simone', 'Baldelli', 'Titolare', 's.baldelli@cma-adriatiche.it', '0712223345', '3381112233', NULL, 1),
(6, 'Elisa', 'Santoni', 'Ufficio Acquisti', 'e.santoni@cma-adriatiche.it', '0712223346', '3381112234', NULL, 1),
(7, 'Fabio', 'Zanetti', 'Direttore Operativo', 'f.zanetti@gsb-spa.it', '0303334456', '3391112233', NULL, 1),
(7, 'Martina', 'Grassi', 'Responsabile Acquisti', 'm.grassi@gsb-spa.it', '0303334457', '3391112234', NULL, 1),
(8, 'Nicola', 'Fumagalli', 'Titolare', 'n.fumagalli@mpb-srl.it', '0355556678', '3401112233', NULL, 1),
(8, 'Alessia', 'Locatelli', 'Amministrazione', 'a.locatelli@mpb-srl.it', '0355556679', '3401112234', NULL, 1),
(9, 'Giorgio', 'Bassi', 'Responsabile Tecnico', 'g.bassi@omf-srl.it', '0432667789', '3411112233', NULL, 1),
(9, 'Ilaria', 'Comelli', 'Ufficio Acquisti', 'i.comelli@omf-srl.it', '0432667790', '3411112234', NULL, 1),
(10, 'Stefano', 'Oliveri', 'Direttore Generale', 's.oliveri@tecnoligure.it', '0107778900', '3421112233', NULL, 1),
(10, 'Cristina', 'Barbieri', 'Responsabile Acquisti', 'c.barbieri@tecnoligure.it', '0107778901', '3421112234', NULL, 1),
(11, 'Enrico', 'Moretti', 'Titolare', 'e.moretti@aiv-srl.it', '0498889901', '3431112233', NULL, 1),
(11, 'Federica', 'Guerra', 'Ufficio Tecnico', 'f.guerra@aiv-srl.it', '0498889902', '3431112234', NULL, 1),
(12, 'Claudio', 'Fiorini', 'Direttore Acquisti', 'c.fiorini@cmu-srl.it', '0759990012', '3441112233', NULL, 1),
(12, 'Giada', 'Antonelli', 'Amministrazione', 'g.antonelli@cmu-srl.it', '0759990013', '3441112234', NULL, 1),
(13, 'Vincenzo', 'De Luca', 'Titolare', 'v.deluca@mgp-srl.it', '0801001123', '3451112233', NULL, 1),
(13, 'Rosa', 'Marino', 'Responsabile Acquisti', 'r.marino@mgp-srl.it', '0801001124', '3451112234', NULL, 1),
(14, 'Michele', 'Dallapiccola', 'Direttore Tecnico', 'm.dallapiccola@fmt-srl.it', '0461112234', '3461112233', NULL, 1),
(14, 'Beatrice', 'Endrizzi', 'Ufficio Acquisti', 'b.endrizzi@fmt-srl.it', '0461112235', '3461112234', NULL, 1),
(15, 'Antonio', 'Esposito', 'Direttore Generale', 'a.esposito@imc-spa.it', '0812223345', '3471112233', NULL, 1),
(15, 'Teresa', 'Russo', 'Responsabile Acquisti', 't.russo@imc-spa.it', '0812223346', '3471112234', NULL, 1);

-- ============================================
-- 4. PRODOTTO (12 righe)
-- ============================================
INSERT INTO Prodotto (nome, descrizione, tipo, codice, prezzo_listino, attivo) VALUES
('Tornio CNC TX-500', 'Tornio a controllo numerico per lavorazioni di precisione', 'Macchinario', 'TCX500', 45000.00, 1),
('Fresa Universale FU-200', 'Fresatrice universale per lavorazioni meccaniche', 'Macchinario', 'FU200', 18500.00, 1),
('Compressore Industriale CI-100', 'Compressore d''aria per uso industriale', 'Macchinario', 'CI100', 8200.00, 1),
('Pressa Idraulica PH-800', 'Pressa idraulica per stampaggio lamiere', 'Macchinario', 'PH800', 62000.00, 1),
('Robot Saldatura RS-50', 'Robot antropomorfo per saldatura automatizzata', 'Macchinario', 'RS50', 39000.00, 1),
('Nastro Trasportatore NT-30', 'Sistema di trasporto materiali su nastro', 'Macchinario', 'NT30', 12500.00, 1),
('Centro di Lavoro CL-1000', 'Centro di lavoro a 5 assi per lavorazioni complesse', 'Macchinario', 'CL1000', 95000.00, 1),
('Piegatrice Lamiera PL-400', 'Piegatrice CNC per lamiere metalliche', 'Macchinario', 'PL400', 27500.00, 1),
('Installazione e Collaudo', 'Servizio di installazione e collaudo macchinario', 'Servizio', 'SRV-INST', 2500.00, 1),
('Manutenzione Annuale Base', 'Contratto di manutenzione annuale base', 'Servizio', 'SRV-MAN01', 1800.00, 1),
('Manutenzione Annuale Premium', 'Contratto di manutenzione annuale con interventi prioritari', 'Servizio', 'SRV-MAN02', 3500.00, 1),
('Formazione Operatori', 'Corso di formazione per operatori macchina', 'Servizio', 'SRV-FORM', 1200.00, 1);

-- ============================================
-- 5. ORDINE (40 righe, distribuite 2024-2026)
-- ============================================
INSERT INTO Ordine (id_azienda_cliente, id_agente, id_contatto_riferimento, data_ordine, stato, note) VALUES
(1, 1, 1, '2024-03-01', 'Vinto', NULL),
(2, 2, 4, '2024-03-21', 'Vinto', NULL),
(3, 3, 5, '2024-04-10', 'InTrattativa', NULL),
(4, 4, NULL, '2024-04-30', 'Perso', NULL),
(5, 5, 9, '2024-05-20', 'Aperto', NULL),
(6, 6, 12, '2024-06-09', 'Vinto', NULL),
(7, 1, 13, '2024-06-29', 'Vinto', NULL),
(8, 2, NULL, '2024-07-19', 'InTrattativa', NULL),
(9, 3, 17, '2024-08-08', 'Perso', NULL),
(10, 4, 20, '2024-08-28', 'Aperto', NULL),
(11, 5, 21, '2024-09-17', 'Vinto', NULL),
(12, 6, NULL, '2024-10-07', 'Vinto', NULL),
(13, 1, 25, '2024-10-27', 'InTrattativa', NULL),
(14, 2, 28, '2024-11-16', 'Perso', NULL),
(15, 3, 29, '2024-12-06', 'Aperto', NULL),
(1, 4, NULL, '2024-12-26', 'Vinto', NULL),
(2, 5, 3, '2025-01-15', 'Vinto', NULL),
(3, 6, 6, '2025-02-04', 'InTrattativa', NULL),
(4, 1, 7, '2025-02-24', 'Perso', NULL),
(5, 2, NULL, '2025-03-16', 'Aperto', NULL),
(6, 3, 11, '2025-04-05', 'Vinto', NULL),
(7, 4, 14, '2025-04-25', 'Vinto', NULL),
(8, 5, 15, '2025-05-15', 'InTrattativa', NULL),
(9, 6, NULL, '2025-06-04', 'Perso', NULL),
(10, 1, 19, '2025-06-24', 'Aperto', NULL),
(11, 2, 22, '2025-07-14', 'Vinto', NULL),
(12, 3, 23, '2025-08-03', 'Vinto', NULL),
(13, 4, NULL, '2025-08-23', 'InTrattativa', NULL),
(14, 5, 27, '2025-09-12', 'Perso', NULL),
(15, 6, 30, '2025-10-02', 'Aperto', NULL),
(1, 1, 1, '2025-10-22', 'Vinto', NULL),
(2, 2, NULL, '2025-11-11', 'Vinto', NULL),
(3, 3, 5, '2025-12-01', 'InTrattativa', NULL),
(4, 4, 8, '2025-12-21', 'Perso', NULL),
(5, 5, 9, '2026-01-10', 'Aperto', NULL),
(6, 6, NULL, '2026-01-30', 'Vinto', NULL),
(7, 1, 13, '2026-02-19', 'Vinto', NULL),
(8, 2, 16, '2026-03-11', 'InTrattativa', NULL),
(9, 3, 17, '2026-03-31', 'Perso', NULL),
(10, 4, NULL, '2026-04-20', 'Aperto', NULL);

-- ============================================
-- 6. RIGAORDINE (65 righe)
-- ============================================
INSERT INTO RigaOrdine (id_ordine, id_prodotto, quantita, prezzo_pattuito, sconto) VALUES
(1, 1, 1, 43000.00, 0), (1, 9, 1, 2500.00, 0),
(2, 2, 1, 17800.00, 0), (2, 10, 1, 1800.00, 0),
(3, 3, 2, 7800.00, 0), (3, 11, 1, 3500.00, 0),
(4, 4, 1, 59000.00, 1000), (4, 9, 1, 2500.00, 0),
(5, 5, 1, 37000.00, 0), (5, 12, 2, 1200.00, 0),
(6, 6, 2, 12000.00, 0), (6, 9, 1, 2500.00, 0),
(7, 7, 1, 90000.00, 2000), (7, 11, 1, 3500.00, 0),
(8, 8, 1, 26000.00, 0), (8, 10, 1, 1800.00, 0),
(9, 1, 1, 44000.00, 0), (9, 9, 1, 2500.00, 0),
(10, 2, 2, 18000.00, 0), (10, 12, 1, 1200.00, 0),
(11, 3, 3, 7900.00, 0), (11, 10, 1, 1800.00, 0),
(12, 4, 1, 60000.00, 0), (12, 11, 1, 3500.00, 0),
(13, 5, 1, 38000.00, 500), (13, 9, 1, 2500.00, 0),
(14, 6, 1, 12200.00, 0), (14, 10, 1, 1800.00, 0),
(15, 7, 1, 93000.00, 0), (15, 12, 2, 1200.00, 0),
(16, 8, 2, 27000.00, 0), (16, 9, 1, 2500.00, 0),
(17, 1, 1, 43500.00, 0), (17, 11, 1, 3500.00, 0),
(18, 2, 1, 18200.00, 0), (18, 10, 1, 1800.00, 0),
(19, 3, 1, 8000.00, 0), (19, 9, 1, 2500.00, 0),
(20, 4, 1, 61000.00, 1500), (20, 12, 1, 1200.00, 0),
(21, 5, 1, 39000.00, 0), (21, 9, 1, 2500.00, 0),
(22, 6, 2, 12300.00, 0), (22, 10, 1, 1800.00, 0),
(23, 7, 1, 94000.00, 0), (23, 11, 1, 3500.00, 0),
(24, 8, 1, 27200.00, 0), (24, 9, 1, 2500.00, 0),
(25, 1, 2, 44500.00, 1000), (25, 12, 1, 1200.00, 0),
(26, 2, 1, 18300.00, 0),
(27, 3, 2, 8100.00, 0),
(28, 4, 1, 62000.00, 0),
(29, 5, 1, 39500.00, 0),
(30, 6, 1, 12500.00, 0),
(31, 7, 1, 95000.00, 0),
(32, 8, 1, 27500.00, 0),
(33, 1, 1, 45000.00, 0),
(34, 2, 1, 18500.00, 0),
(35, 3, 1, 8200.00, 0),
(36, 4, 1, 62000.00, 0),
(37, 5, 1, 39000.00, 0),
(38, 6, 1, 12500.00, 0),
(39, 7, 1, 95000.00, 0),
(40, 8, 1, 27500.00, 0);

-- ============================================
-- 7. LOGATTIVITA (70 righe: 2 per ordine 1-30 + 10 esplorative)
-- ============================================
INSERT INTO LogAttivita (id_ordine, id_contatto, id_agente, data_ora, tipo_attivita, oggetto, descrizione, esito, allegato_url) VALUES
(1, 1, 1, '2024-02-20', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(1, 1, 1, '2024-03-06', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(2, 4, 2, '2024-03-11', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(2, 4, 2, '2024-03-26', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(3, 5, 3, '2024-03-31', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(3, 5, 3, '2024-04-15', 'Chiamata', 'Negoziazione condizioni commerciali', NULL, 'Trattativa in corso', NULL),
(4, NULL, 4, '2024-04-20', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(4, NULL, 4, '2024-05-05', 'Email', 'Comunicazione esito negativo', NULL, 'Trattativa persa', NULL),
(5, 9, 5, '2024-05-10', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(5, 9, 5, '2024-05-25', 'Email', 'Invio offerta commerciale', NULL, 'In attesa di riscontro', NULL),
(6, 12, 6, '2024-05-30', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(6, 12, 6, '2024-06-14', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(7, 13, 1, '2024-06-19', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(7, 13, 1, '2024-07-04', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(8, NULL, 2, '2024-07-09', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(8, NULL, 2, '2024-07-24', 'Chiamata', 'Negoziazione condizioni commerciali', NULL, 'Trattativa in corso', NULL),
(9, 17, 3, '2024-07-29', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(9, 17, 3, '2024-08-13', 'Email', 'Comunicazione esito negativo', NULL, 'Trattativa persa', NULL),
(10, 20, 4, '2024-08-18', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(10, 20, 4, '2024-09-02', 'Email', 'Invio offerta commerciale', NULL, 'In attesa di riscontro', NULL),
(11, 21, 5, '2024-09-07', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(11, 21, 5, '2024-09-22', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(12, NULL, 6, '2024-09-27', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(12, NULL, 6, '2024-10-12', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(13, 25, 1, '2024-10-17', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(13, 25, 1, '2024-11-01', 'Chiamata', 'Negoziazione condizioni commerciali', NULL, 'Trattativa in corso', NULL),
(14, 28, 2, '2024-11-06', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(14, 28, 2, '2024-11-21', 'Email', 'Comunicazione esito negativo', NULL, 'Trattativa persa', NULL),
(15, 29, 3, '2024-11-26', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(15, 29, 3, '2024-12-11', 'Email', 'Invio offerta commerciale', NULL, 'In attesa di riscontro', NULL),
(16, NULL, 4, '2024-12-16', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(16, NULL, 4, '2024-12-31', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(17, 3, 5, '2025-01-05', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(17, 3, 5, '2025-01-20', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(18, 6, 6, '2025-01-25', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(18, 6, 6, '2025-02-09', 'Chiamata', 'Negoziazione condizioni commerciali', NULL, 'Trattativa in corso', NULL),
(19, 7, 1, '2025-02-14', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(19, 7, 1, '2025-03-01', 'Email', 'Comunicazione esito negativo', NULL, 'Trattativa persa', NULL),
(20, NULL, 2, '2025-03-06', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(20, NULL, 2, '2025-03-21', 'Email', 'Invio offerta commerciale', NULL, 'In attesa di riscontro', NULL),
(21, 11, 3, '2025-03-26', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(21, 11, 3, '2025-04-10', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(22, 14, 4, '2025-04-15', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(22, 14, 4, '2025-04-30', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(23, 15, 5, '2025-05-05', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(23, 15, 5, '2025-05-20', 'Chiamata', 'Negoziazione condizioni commerciali', NULL, 'Trattativa in corso', NULL),
(24, NULL, 6, '2025-05-25', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(24, NULL, 6, '2025-06-09', 'Email', 'Comunicazione esito negativo', NULL, 'Trattativa persa', NULL),
(25, 19, 1, '2025-06-14', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(25, 19, 1, '2025-06-29', 'Email', 'Invio offerta commerciale', NULL, 'In attesa di riscontro', NULL),
(26, 22, 2, '2025-07-04', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(26, 22, 2, '2025-07-19', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(27, 23, 3, '2025-07-24', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(27, 23, 3, '2025-08-08', 'Incontro', 'Firma contratto', NULL, 'Ordine confermato', NULL),
(28, NULL, 4, '2025-08-13', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(28, NULL, 4, '2025-08-28', 'Chiamata', 'Negoziazione condizioni commerciali', NULL, 'Trattativa in corso', NULL),
(29, 27, 5, '2025-09-02', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(29, 27, 5, '2025-09-17', 'Email', 'Comunicazione esito negativo', NULL, 'Trattativa persa', NULL),
(30, 30, 6, '2025-09-22', 'Chiamata', 'Primo contatto commerciale', NULL, 'Interesse confermato', NULL),
(30, 30, 6, '2025-10-07', 'Email', 'Invio offerta commerciale', NULL, 'In attesa di riscontro', NULL),
(NULL, 1, 1, '2026-05-01', 'Chiamata', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 4, 2, '2026-05-15', 'Email', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 7, 3, '2026-05-29', 'Chiamata', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 10, 4, '2026-06-12', 'Email', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 13, 5, '2026-06-26', 'Chiamata', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 16, 6, '2026-07-10', 'Email', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 19, 1, '2026-07-24', 'Chiamata', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 22, 2, '2026-08-01', 'Email', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 25, 3, '2026-08-07', 'Chiamata', 'Contatto esplorativo', NULL, 'Da valutare', NULL),
(NULL, 28, 4, '2026-08-14', 'Email', 'Contatto esplorativo', NULL, 'Da valutare', NULL);

CREATE TABLE dbo.CustomerAccounts (
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_CustomerAccounts_Id DEFAULT NEWSEQUENTIALID(),
    LegalName NVARCHAR(200) NOT NULL,
    TaxId NVARCHAR(14) NOT NULL,
    BranchCode NVARCHAR(10) NOT NULL,
    AccountNumber NVARCHAR(20) NOT NULL,
    AccountCheckDigit NVARCHAR(2) NULL,
    IspbCode CHAR(8) NULL,
    Status TINYINT NOT NULL CONSTRAINT DF_CustomerAccounts_Status DEFAULT 1,
    CreatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_CustomerAccounts_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2(3) NULL,
    CONSTRAINT PK_CustomerAccounts PRIMARY KEY (Id),
    CONSTRAINT UQ_CustomerAccounts_BranchAccount UNIQUE (BranchCode, AccountNumber, AccountCheckDigit)
);

CREATE TABLE dbo.CustomerAccountConfigurations (
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_CustomerAccountConfigurations_Id DEFAULT NEWSEQUENTIALID(),
    CustomerAccountId UNIQUEIDENTIFIER NOT NULL,
    DailyPixOutgoingLimit DECIMAL(19,2) NULL,
    PerTransactionOutgoingLimit DECIMAL(19,2) NULL,
    WebhookUrl NVARCHAR(2048) NULL,
    WebhookSigningSecret NVARCHAR(256) NULL,
    PixKeyDefault NVARCHAR(77) NULL,
    AllowDynamicQr BIT NOT NULL CONSTRAINT DF_CustomerAccountConfigurations_AllowDynamicQr DEFAULT 1,
    AllowStaticQr BIT NOT NULL CONSTRAINT DF_CustomerAccountConfigurations_AllowStaticQr DEFAULT 1,
    MetadataJson NVARCHAR(MAX) NULL,
    UpdatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_CustomerAccountConfigurations_UpdatedAtUtc DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_CustomerAccountConfigurations PRIMARY KEY (Id),
    CONSTRAINT FK_CustomerAccountConfigurations_CustomerAccounts FOREIGN KEY (CustomerAccountId) REFERENCES dbo.CustomerAccounts (Id),
    CONSTRAINT UQ_CustomerAccountConfigurations_Account UNIQUE (CustomerAccountId)
);

CREATE TABLE dbo.ApiUsers (
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_ApiUsers_Id DEFAULT NEWSEQUENTIALID(),
    CustomerAccountId UNIQUEIDENTIFIER NULL,
    Email NVARCHAR(320) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    ApiClientId NVARCHAR(64) NULL,
    ApiClientSecretHash NVARCHAR(500) NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_ApiUsers_IsActive DEFAULT 1,
    FailedLoginCount INT NOT NULL CONSTRAINT DF_ApiUsers_FailedLoginCount DEFAULT 0,
    LockoutEndUtc DATETIME2(3) NULL,
    CreatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_ApiUsers_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
    LastLoginAtUtc DATETIME2(3) NULL,
    CONSTRAINT PK_ApiUsers PRIMARY KEY (Id),
    CONSTRAINT FK_ApiUsers_CustomerAccounts FOREIGN KEY (CustomerAccountId) REFERENCES dbo.CustomerAccounts (Id),
    CONSTRAINT UQ_ApiUsers_Email UNIQUE (Email),
    CONSTRAINT UQ_ApiUsers_ApiClientId UNIQUE (ApiClientId)
);

CREATE TABLE dbo.Transactions (
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_Transactions_Id DEFAULT NEWSEQUENTIALID(),
    CustomerAccountId UNIQUEIDENTIFIER NOT NULL,
    IdempotencyKey NVARCHAR(64) NOT NULL,
    TransactionType TINYINT NOT NULL,
    Direction TINYINT NOT NULL,
    Status TINYINT NOT NULL,
    Amount DECIMAL(19,2) NOT NULL,
    CurrencyCode CHAR(3) NOT NULL CONSTRAINT DF_Transactions_CurrencyCode DEFAULT 'BRL',
    Txid NVARCHAR(35) NULL,
    EndToEndId NVARCHAR(32) NULL,
    PayerTaxId NVARCHAR(14) NULL,
    PayeeTaxId NVARCHAR(14) NULL,
    PayerName NVARCHAR(200) NULL,
    PayeeName NVARCHAR(200) NULL,
    ErrorCode NVARCHAR(64) NULL,
    ErrorMessage NVARCHAR(1000) NULL,
    RequestPayloadJson NVARCHAR(MAX) NULL,
    ResponsePayloadJson NVARCHAR(MAX) NULL,
    CreatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_Transactions_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
    SettledAtUtc DATETIME2(3) NULL,
    CONSTRAINT PK_Transactions PRIMARY KEY (Id),
    CONSTRAINT FK_Transactions_CustomerAccounts FOREIGN KEY (CustomerAccountId) REFERENCES dbo.CustomerAccounts (Id),
    CONSTRAINT UQ_Transactions_Idempotency UNIQUE (IdempotencyKey)
);

CREATE INDEX IX_Transactions_CustomerAccountId_CreatedAtUtc ON dbo.Transactions (CustomerAccountId, CreatedAtUtc DESC);
CREATE INDEX IX_Transactions_Txid ON dbo.Transactions (Txid) WHERE Txid IS NOT NULL;
CREATE INDEX IX_Transactions_EndToEndId ON dbo.Transactions (EndToEndId) WHERE EndToEndId IS NOT NULL;

CREATE TABLE dbo.QrCodeRecords (
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_QrCodeRecords_Id DEFAULT NEWSEQUENTIALID(),
    CustomerAccountId UNIQUEIDENTIFIER NOT NULL,
    Txid NVARCHAR(35) NULL,
    QrKind TINYINT NOT NULL,
    EmvPayload NVARCHAR(MAX) NOT NULL,
    Amount DECIMAL(19,2) NULL,
    Description NVARCHAR(140) NULL,
    ExpiresAtUtc DATETIME2(3) NULL,
    Status TINYINT NOT NULL,
    RelatedTransactionId UNIQUEIDENTIFIER NULL,
    CreatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_QrCodeRecords_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_QrCodeRecords PRIMARY KEY (Id),
    CONSTRAINT FK_QrCodeRecords_CustomerAccounts FOREIGN KEY (CustomerAccountId) REFERENCES dbo.CustomerAccounts (Id),
    CONSTRAINT FK_QrCodeRecords_Transactions FOREIGN KEY (RelatedTransactionId) REFERENCES dbo.Transactions (Id)
);

CREATE INDEX IX_QrCodeRecords_CustomerAccountId_CreatedAtUtc ON dbo.QrCodeRecords (CustomerAccountId, CreatedAtUtc DESC);
CREATE INDEX IX_QrCodeRecords_Txid ON dbo.QrCodeRecords (Txid) WHERE Txid IS NOT NULL;

CREATE TABLE dbo.TransactionRefundLinks (
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_TransactionRefundLinks_Id DEFAULT NEWSEQUENTIALID(),
    OriginalTransactionId UNIQUEIDENTIFIER NOT NULL,
    RefundTransactionId UNIQUEIDENTIFIER NOT NULL,
    RefundEndToEndId NVARCHAR(32) NULL,
    Amount DECIMAL(19,2) NOT NULL,
    ReasonCode NVARCHAR(32) NULL,
    CreatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_TransactionRefundLinks_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_TransactionRefundLinks PRIMARY KEY (Id),
    CONSTRAINT FK_TransactionRefundLinks_Original FOREIGN KEY (OriginalTransactionId) REFERENCES dbo.Transactions (Id),
    CONSTRAINT FK_TransactionRefundLinks_Refund FOREIGN KEY (RefundTransactionId) REFERENCES dbo.Transactions (Id),
    CONSTRAINT CHK_TransactionRefundLinks_Different CHECK (OriginalTransactionId <> RefundTransactionId),
    CONSTRAINT UQ_TransactionRefundLinks_Refund UNIQUE (RefundTransactionId)
);

CREATE INDEX IX_TransactionRefundLinks_Original ON dbo.TransactionRefundLinks (OriginalTransactionId);

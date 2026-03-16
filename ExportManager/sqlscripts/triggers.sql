CREATE TRIGGER trg_OrderItems_Insert  ON OrderItems  AFTER INSERT 
AS
BEGIN 
UPDATE OI 
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM OrderItems OI
INNER JOIN inserted i ON OI.OrderItemId = i.OrderItemId;
END;
GO
CREATE TRIGGER trg_OrderItems_Update  ON OrderItems  AFTER UPDATE
AS
BEGIN
UPDATE OI
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM OrderItems OI
INNER JOIN inserted i ON OI.OrderItemId = i.OrderItemId;
END;
GO
CREATE TRIGGER trg_Orders_Insert  ON Orders  AFTER INSERT
AS
BEGIN
UPDATE O
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM Orders O
INNER JOIN inserted i ON O.OrderId = i.OrderId;
END;
GO 
CREATE TRIGGER trg_Orders_Update  ON Orders  AFTER UPDATE
AS
BEGIN
UPDATE O
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM Orders O
INNER JOIN inserted i ON O.OrderId = i.OrderId;
END;
GO 
CREATE TRIGGER trg_PaymentMethods_Insert  ON PaymentMethods  AFTER INSERT
AS
BEGIN
UPDATE PM
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM PaymentMethods PM
INNER JOIN inserted i ON PM.PaymentMethodId = i.PaymentMethodId;
END;
GO 
CREATE TRIGGER trg_PaymentMethods_Update  ON PaymentMethods  AFTER UPDATE
AS
BEGIN
UPDATE PM
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM PaymentMethods PM
INNER JOIN inserted i ON PM.PaymentMethodId = i.PaymentMethodId;
END;
GO 
CREATE TRIGGER trg_Invoices_Insert  ON Invoices  AFTER INSERT
AS
BEGIN
UPDATE INV
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM Invoices INV
INNER JOIN inserted i ON INV.InvoiceId = i.InvoiceId;
END;
GO 
CREATE TRIGGER trg_Invoices_Update  ON Invoices  AFTER UPDATE
AS
BEGIN
UPDATE INV
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM Invoices INV
INNER JOIN inserted i ON INV.InvoiceId = i.InvoiceId;
END;
GO 
CREATE TRIGGER trg_Countries_Insert  ON Countries
AFTER INSERT
AS
BEGIN
UPDATE Countries
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM Countries AS C
INNER JOIN inserted i ON C.CountryId = i.CountryId;
END;
GO 
CREATE TRIGGER trg_Addresses_Insert  ON Addresses
AFTER INSERT
AS
BEGIN
UPDATE Addresses
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM Addresses AS A
INNER JOIN inserted i ON A.AddressId = i.AddressId;
END;
GO 
CREATE TRIGGER trg_Addresses_Update  ON Addresses
AFTER UPDATE
AS
BEGIN
UPDATE Addresses
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM Addresses A
INNER JOIN inserted i ON A.AddressId = i.AddressId;
END;
GO 
CREATE TRIGGER trg_Countries_Update  ON Countries
AFTER UPDATE
AS
BEGIN
UPDATE Countries
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM Countries A
INNER JOIN inserted i ON A.CountryId = i.CountryId;
END;
GO 
CREATE TRIGGER TR_StockMovements_Insert  ON StockMovements
AFTER INSERT
AS
BEGIN
SET NOCOUNT ON;
UPDATE sm
SET
CreatedBy = SUSER_SNAME(),
CreatedAt = SYSUTCDATETIME()
FROM StockMovements sm
INNER JOIN inserted i ON sm.StockMovementId = i.StockMovementId;
END;
GO 
CREATE TRIGGER TR_StockMovements_Update  ON StockMovements
AFTER UPDATE
AS
BEGIN
SET NOCOUNT ON;
UPDATE sm
SET
UpdatedBy = SUSER_SNAME(),
UpdatedAt = SYSUTCDATETIME()
FROM StockMovements sm
INNER JOIN inserted i ON sm.StockMovementId = i.StockMovementId;
END;
GO 
CREATE TRIGGER trg_Growers_Insert  ON Growers
AFTER INSERT
AS
BEGIN
UPDATE G
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM Growers G
INNER JOIN inserted i ON G.GrowerId = i.GrowerId;
END;
GO 
CREATE TRIGGER trg_Growers_Update  ON Growers
AFTER UPDATE
AS
BEGIN
UPDATE G
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM Growers G
INNER JOIN inserted i ON G.GrowerId = i.GrowerId;
END;
GO 
CREATE TRIGGER trg_Clients_Insert  ON Clients
AFTER INSERT
AS
BEGIN
UPDATE C
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM Clients C
INNER JOIN inserted i ON C.ClientId = i.ClientId;
END;
GO 
CREATE TRIGGER trg_Clients_Update  ON Clients
AFTER UPDATE
AS
BEGIN
UPDATE C
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM Clients C
INNER JOIN inserted i ON C.ClientId = i.ClientId;
END;
GO 
CREATE TRIGGER trg_TrayTypes_Insert  ON TrayTypes
AFTER INSERT
AS
BEGIN
UPDATE T
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM TrayTypes T
INNER JOIN inserted i ON T.TrayTypeId = i.TrayTypeId;
END;
GO 
CREATE TRIGGER trg_TrayTypes_Update  ON TrayTypes
AFTER UPDATE
AS
BEGIN
UPDATE T
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM TrayTypes T
INNER JOIN inserted i ON T.TrayTypeId = i.TrayTypeId;
END;
GO 
CREATE TRIGGER trg_CarrierTypes_Insert  ON CarrierTypes
AFTER INSERT
AS
BEGIN
UPDATE C
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM CarrierTypes C
INNER JOIN inserted i ON C.CarrierTypeId = i.CarrierTypeId;
END;
GO 
CREATE TRIGGER trg_CarrierTypes_Update  ON CarrierTypes
AFTER UPDATE
AS
BEGIN
UPDATE C
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM CarrierTypes C
INNER JOIN inserted i ON C.CarrierTypeId = i.CarrierTypeId;
END;
GO 
CREATE TRIGGER trg_Colors_Insert  ON Colors
AFTER INSERT
AS
BEGIN
UPDATE C
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()
FROM Colors C
INNER JOIN inserted i ON C.ColorId = i.ColorId;
END;
GO 
CREATE TRIGGER trg_Colors_Update  ON Colors
AFTER UPDATE
AS
BEGIN
UPDATE C
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM Colors C
INNER JOIN inserted i ON C.ColorId = i.ColorId;
END;
GO 
CREATE TRIGGER trg_Categories_Insert  ON Categories
AFTER INSERT
AS
BEGIN
UPDATE C
SET CreatedAt = SYSUTCDATETIME(),
CreatedBy = SUSER_SNAME()      
FROM Categories C
INNER JOIN inserted i ON C.CategoryId = i.CategoryId;
END;
GO 
CREATE TRIGGER trg_Categories_Update  ON Categories
AFTER UPDATE
AS
BEGIN
UPDATE C 
SET UpdatedAt = SYSUTCDATETIME(),
UpdatedBy = SUSER_SNAME()
FROM Categories C
INNER JOIN inserted i ON C.CategoryId = i.CategoryId;
END;
GO 
CREATE TRIGGER trg_Qualities_Insert  ON Qualities
AFTER INSERT
AS
BEGIN
UPDATE Q 
SET CreatedAt = SYSUTCDATETIME(), 
CreatedBy = SUSER_SNAME() 
FROM Qualities Q      INNER JOIN inserted i ON Q.QualityId = i.QualityId;
END;
GO 
CREATE TRIGGER TR_InvoiceItems_Insert  ON dbo.InvoiceItems
AFTER INSERT
AS
BEGIN
SET NOCOUNT ON;    
UPDATE ii  
SET      
CreatedAt = ISNULL(ii.CreatedAt, SYSDATETIME()), 
CreatedBy = ISNULL(ii.CreatedBy, SUSER_SNAME()) 
FROM dbo.InvoiceItems ii    
INNER JOIN inserted ins ON ii.InvoiceItemId = ins.InvoiceItemId;
END;   
GO 
CREATE TRIGGER trg_Qualities_Update  ON Qualities 
AFTER UPDATE 
AS  
BEGIN   
UPDATE Q  
SET
UpdatedAt = SYSUTCDATETIME(),     
UpdatedBy = SUSER_SNAME()  
FROM Qualities Q     
INNER JOIN inserted i ON Q.QualityId = i.QualityId; 
END;   
GO 
CREATE TRIGGER TR_InvoiceItems_Update  ON dbo.InvoiceItems 
AFTER UPDATE
AS 
BEGIN  
SET NOCOUNT ON;  
UPDATE ii   
SET        
UpdatedAt = SYSDATETIME(), 
UpdatedBy = ISNULL(ii.UpdatedBy, SUSER_SNAME())   
FROM dbo.InvoiceItems ii   
INNER JOIN inserted ins ON ii.InvoiceItemId = ins.InvoiceItemId;
END;  
GO 
CREATE TRIGGER TR_InvoiceParties_Insert  ON dbo.InvoiceParties 
AFTER INSERT 
AS 
BEGIN 
SET NOCOUNT ON;     
UPDATE ip     
SET         
CreatedAt = ISNULL(ip.CreatedAt, SYSDATETIME()),    
CreatedBy = ISNULL(ip.CreatedBy, SUSER_SNAME()) 
FROM dbo.InvoiceParties ip 
INNER JOIN inserted ins ON ip.InvoicePartyId = ins.InvoicePartyId;
END;
GO 
CREATE TRIGGER TR_InvoiceParties_Update  ON dbo.InvoiceParties  
AFTER UPDATE  
AS  
BEGIN 
SET NOCOUNT ON;  
UPDATE ip  
SET        
UpdatedAt = ISNULL(ip.UpdatedAt, SYSDATETIME()),  
UpdatedBy = ISNULL(ip.UpdatedBy, SUSER_SNAME())
FROM dbo.InvoiceParties ip  
INNER JOIN inserted ins ON ip.InvoicePartyId = ins.InvoicePartyId;
END;  
GO 
CREATE TRIGGER TR_Carriers_Insert  ON dbo.Carriers 
AFTER INSERT  
AS
BEGIN  
SET NOCOUNT ON;    
UPDATE c    
SET        
CreatedAt = ISNULL(c.CreatedAt, SYSDATETIME()), 
CreatedBy = ISNULL(c.CreatedBy, SUSER_SNAME())  
FROM dbo.Carriers c   
INNER JOIN inserted i     
ON c.CarrierId = i.CarrierId; 
END;  
GO 
CREATE TRIGGER trg_Products_Insert  ON Products  
AFTER INSERT 
AS
BEGIN   
UPDATE P  
SET CreatedAt = SYSUTCDATETIME(),   
CreatedBy = SUSER_SNAME()   
FROM Products P   
INNER JOIN inserted i ON P.ProductId = i.ProductId; 
END;  
GO 
CREATE TRIGGER TR_Carriers_Update  ON dbo.Carriers 
AFTER UPDATE 
AS 
BEGIN 
SET NOCOUNT ON;      
UPDATE c   
SET         
UpdatedAt = SYSDATETIME(),  
UpdatedBy = SUSER_SNAME() 
FROM dbo.Carriers c     
INNER JOIN inserted i     
ON c.CarrierId = i.CarrierId   
INNER JOIN deleted d       
ON d.CarrierId = c.CarrierId  
END;  
GO 
CREATE TRIGGER trg_Products_Update  ON Products  
AFTER UPDATE 
AS 
BEGIN   
UPDATE P   
SET UpdatedAt = SYSUTCDATETIME(),  
UpdatedBy = SUSER_SNAME()  
FROM Products P     
INNER JOIN inserted i ON P.ProductId = i.ProductId; 
END;  
GO
--CREATE TRIGGER trg_OrderItems_InternalNo
--ON OrderItems
--AFTER INSERT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    WITH Numbered AS
--    (
--        SELECT 
--            OI.OrderItemId,
--            RN = ROW_NUMBER() OVER (PARTITION BY OI.OrderId ORDER BY OI.OrderItemId)
--        FROM OrderItems OI
--        INNER JOIN inserted i ON OI.OrderItemId = i.OrderItemId
--    )
--    UPDATE OI
--    SET InternalNo = N.RN
--    FROM OrderItems OI
--    INNER JOIN Numbered N ON OI.OrderItemId = N.OrderItemId;
--END;
--GO
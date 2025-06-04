create database QuanLyCuaHang
go

-- Bảng lưu trữ thông tin về cửa hàng
CREATE TABLE Stores (
    StoreID INT PRIMARY KEY IDENTITY,
    StoreName NVARCHAR(100),
    Location NVARCHAR(255)
);

-- Bảng lưu trữ thông tin về nhân viên
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Position NVARCHAR(100),
    StoreID INT, -- Thay thế OperatingBase bằng StoreID
    Salary FLOAT,
    HireDate DATE,
    FOREIGN KEY (StoreID) REFERENCES Stores(StoreID) -- Khóa ngoại tham chiếu đến StoreID trong bảng Stores
);
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY,
    ProductName NVARCHAR(100),
    Description NVARCHAR(255),
    Price FLOAT
);

CREATE TABLE ProductStores (
    ProductID INT,
    StoreID INT,
    QuantityInStock INT DEFAULT 0,
    PRIMARY KEY (ProductID, StoreID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (StoreID) REFERENCES Stores(StoreID)
);

-- Bảng lưu trữ thông tin về đơn xuất nhập sản phẩm
CREATE TABLE PurchaseRequests (
    RequestID INT PRIMARY KEY IDENTITY,
    RequestDate DATETIME,
    ProductID INT,
    QuantityRequested INT check (QuantityRequested>0),
    RequestedByEmployeeID INT,
    RequestStatus NVARCHAR(20) CHECK (RequestStatus IN (N'Đã yêu cầu', N'Đã hủy', N'Đã chấp nhận')),
    StoreID INT,
    RequestType NVARCHAR(20) CHECK (RequestType IN (N'Xuất kho', N'Nhập kho')),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (RequestedByEmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (StoreID) REFERENCES Stores(StoreID)
);


-- Bảng lưu trữ thông tin về tài khoản
CREATE TABLE Accounts (
    Username VARCHAR(50) PRIMARY KEY,
    Password VARCHAR(100),
    EmployeeID INT,
    PositionID INT,
	Available BIT,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);	
CREATE PROCEDURE GetProductDetails
AS
BEGIN
    SELECT 
        P.ProductID, 
        P.ProductName, 
        P.Description, 
        P.Price, 
		ISNULL(PS.QuantityInStock, 0) AS QuantityInStock
    FROM 
        Products AS P
    LEFT JOIN 
        ProductStores AS PS ON P.ProductID = PS.ProductID;
END;
CREATE PROCEDURE GetEmployeeDetails
AS
BEGIN
    SELECT 
        E.EmployeeID, 
        E.FirstName, 
        E.LastName, 
        E.Position,
		E.Salary,
		E.HireDate,
        S.StoreName AS StoreName,
		a.username as UserName,
        CASE 
            WHEN A.Available = 1 THEN N'Khả dụng'
            ELSE N'Không khả dụng'
        END AS AccountStatus
    FROM 
        Employees AS E
    INNER JOIN 
        Stores AS S ON E.StoreID = S.StoreID
    LEFT JOIN 
        Accounts AS A ON E.EmployeeID = A.EmployeeID;
END;

CREATE TRIGGER trg_AddAccountOnEmployeeInsert
ON Employees
AFTER INSERT
AS
BEGIN
    DECLARE @EmployeeID INT;
    DECLARE @FirstName NVARCHAR(50);
    DECLARE @LastName NVARCHAR(50);
    DECLARE @HireDate DATE;
    DECLARE @StoreID INT;
    DECLARE @PositionID INT;
    DECLARE @Username NVARCHAR(150); -- Username dựa trên ngày và "cuahang"
    DECLARE @Password NVARCHAR(100) = 'chuoicuahang'; -- Mật khẩu mặc định

    SELECT @EmployeeID = EmployeeID, @FirstName = FirstName, @LastName = LastName, @HireDate = HireDate, @StoreID = StoreID
    FROM inserted;

    -- Tạo username từ "cuahang" và ngày
    SET @Username = CONCAT('cuahang', FORMAT(@HireDate, 'ddMMyyyy'),@EmployeeID);

    -- Lấy PositionID
    SET @PositionID = 1; -- Giả sử PositionID mặc định là 1

    -- Thêm vào bảng Accounts
    INSERT INTO Accounts (Username, Password, EmployeeID, PositionID, Available)
    VALUES (@Username, @Password, @EmployeeID, @PositionID, 1);
END;
INSERT INTO Stores (StoreName, Location)
VALUES 
(N'Bách Hóa Xanh 1', N'Địa chỉ 1'),
(N'Bách Hóa Xanh 2', N'Địa chỉ 2'),
(N'Bách Hóa Xanh 3', N'Địa chỉ 3'),
(N'Bách Hóa Xanh 4', N'Địa chỉ 4'),
(N'Bách Hóa Xanh 5', N'Địa chỉ 5'),
(N'Bách Hóa Xanh 6', N'Địa chỉ 6'),
(N'Bách Hóa Xanh 7', N'Địa chỉ 7'),
(N'Bách Hóa Xanh 8', N'Địa chỉ 8'),
(N'Bách Hóa Xanh 9', N'Địa chỉ 9'),
(N'Bách Hóa Xanh 10',N'Địa chỉ 10');
create PROCEDURE GetStoreInfo
AS
BEGIN
    -- Số lượng loại sản phẩm
    SELECT COUNT(ProductID) AS NumberOfProductTypes
    FROM Products;

    -- Tổng số lượng sản phẩm tồn kho
    SELECT COUNT(StoreID) AS TotalStores FROM Stores

    -- Số lượng nhân viên
    SELECT COUNT(EmployeeID) AS NumberOfEmployees
    FROM Employees;

    -- Số lượng yêu cầu nhập hàng
    SELECT COUNT(RequestID) AS NumberOfPurchaseRequests
    FROM PurchaseRequests
    WHERE RequestStatus = N'Đã yêu cầu';
END;
insert into Accounts(Username,Password,PositionID,Available) values ('admin','admin',0,1)

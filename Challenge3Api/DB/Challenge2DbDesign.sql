CREATE TABLE IF NOT EXISTS DrinkCategories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Category_ID INTEGER NOT NULL,
    Flavor TEXT,
    SKU TEXT NOT NULL,
    Volume INTEGER NOT NULL,
    Expiration_In_Days INTEGER NOT NULL,
    PackagingType TEXT CHECK (PackagingType IN ('Bottle', 'Can')) NOT NULL,
    FOREIGN KEY (Category_ID) REFERENCES DrinkCategories(Id)
);

CREATE TABLE IF NOT EXISTS Packaging (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PackageSize INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS ProductPackaging (
    Packaging_Id INTEGER NOT NULL,
    Product_Id INTEGER NOT NULL,
    FOREIGN KEY (Product_Id) REFERENCES Products(Id),
    FOREIGN KEY (Packaging_Id) REFERENCES Packaging(Id)
);

INSERT OR IGNORE INTO DrinkCategories (Name) VALUES
    ('beer'),
    ('hard seltzer'),
    ('cider'),
    ('canned cocktail');
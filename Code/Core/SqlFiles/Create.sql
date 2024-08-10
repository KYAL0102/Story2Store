CREATE TABLE IF NOT EXISTS "User" (
    "Username" TEXT NOT NULL PRIMARY KEY ,
    "Password" TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Story" (
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Username" INTEGER NOT NULL,
    FOREIGN KEY ("Username") REFERENCES "User" ("Username")
);

CREATE TABLE IF NOT EXISTS "TextComponent" (
    "Guid" VARCHAR(255) PRIMARY KEY,
    "Content" TEXT NOT NULL,
    "Type" TEXT NOT NULL,
    "StoryId" INTEGER NOT NULL,
    FOREIGN KEY ("StoryId") REFERENCES "Story" ("Id")
);
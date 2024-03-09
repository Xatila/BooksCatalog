CREATE schema `books`;
USE `books`;
CREATE TABLE bookinfo (
    `id` INT PRIMARY KEY,
    `Title` VARCHAR(255) NOT NULL,
    `Description` VARCHAR(255) NOT NULL
);
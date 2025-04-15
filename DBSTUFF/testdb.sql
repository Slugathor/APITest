-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 15, 2025 at 05:05 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `testdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `id` int(16) NOT NULL,
  `username` varchar(127) NOT NULL,
  `password_hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `accounts`
--

INSERT INTO `accounts` (`id`, `username`, `password_hash`) VALUES
(1, 'onesInNames', ''),
(2, 'twosInNames', ''),
(3, 'threesInNames', ''),
(7, 'myuser', '$2y$10$ovM9gCr7o8RI7KaWhZhQFuezWnredAW5dm1W2e5yDR9IbpTkYw60.'),
(8, 'testacc2', '$2y$10$A01ium7NxWK65pSsx3QlbuSoxYdcKbU/Z3hHaHeDJ06s6ap4a9P0S'),
(10, 'anotherone', '$2y$10$7ir4jGDPP.cQq9jRPiq9U.9r5x/k.p3THm1uC/WauqGFBJd.7XoKq');

-- --------------------------------------------------------

--
-- Table structure for table `characters`
--

CREATE TABLE `characters` (
  `id` int(11) NOT NULL,
  `account_id` int(16) NOT NULL,
  `name` varchar(16) NOT NULL,
  `level` int(16) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `characters`
--

INSERT INTO `characters` (`id`, `account_id`, `name`, `level`) VALUES
(1, 1, 'WarriorOne', 13),
(2, 1, 'MageOne', 20),
(3, 1, 'RogueOne', 111),
(4, 3, 'ByThreeTheyCome', 33),
(5, 2, 'TwoMinutesToMidn', 2358),
(6, 2, 'TwoGoodTwoBeTrue', 222),
(7, 3, 'ThreeMan', 31),
(8, 1, 'NoOne', 1),
(9, 3, 'Threedom', 52);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`username`);

--
-- Indexes for table `characters`
--
ALTER TABLE `characters`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `accounts`
--
ALTER TABLE `accounts`
  MODIFY `id` int(16) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `characters`
--
ALTER TABLE `characters`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

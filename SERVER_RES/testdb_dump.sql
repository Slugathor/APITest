-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 13, 2025 at 04:53 PM
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
  `password_hash` varchar(255) NOT NULL,
  `accountType` int(2) NOT NULL DEFAULT 0 COMMENT '0=regular user\r\n1=admin'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `accounts`
--

INSERT INTO `accounts` (`id`, `username`, `password_hash`, `accountType`) VALUES
(7, 'myuser', '$2y$10$ovM9gCr7o8RI7KaWhZhQFuezWnredAW5dm1W2e5yDR9IbpTkYw60.', 0),
(8, 'testacc2', '$2y$10$A01ium7NxWK65pSsx3QlbuSoxYdcKbU/Z3hHaHeDJ06s6ap4a9P0S', 0),
(10, 'james', '$2y$10$hHG8lgWg5dwR0ZbsowbGlOhE9oS6QBmULQTamcMVjwFueMQJRDaTi', 0),
(11, 'testi2', '$2y$10$jth9JV1hsr0TBT/28l0rX.1EQ1GezxcOwD6C5I3TB7TYAJWcbuMiG', 1),
(12, 'sami', '$2y$10$h6fIPM/r3TIOszj8934AVuHpKBq1u6f4Aj18RilzrSB9RnMhupe9K', 0),
(13, 'uusi', '$2y$10$Llm6zPwb762lDgaQNuxtAOyRukt7JmMSv5XfGAXGMzng0D7gin.Y6', 0),
(14, 'admin', '$2y$10$swmDLNHMh81GKaRxOQWkiuD6Gvo3bIk/2.QdtjIYJ6krKlcZR6j1K', 1),
(15, 'helmeri', '$2y$10$/0B7fTNv/dnhU1pL/IsgXuhSCqy89cJe757yDXPhl9NwTQaYZjAuC', 0),
(16, 'testuser', '$2y$10$tp1k5gHxMeYlRz9VYvgv2e9ThzEwXPGPCdy3kElAb7n0J9mFap1JW', 0);

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
(10, 11, 'testihahmo', 33),
(16, 8, 'poistamut', 66),
(18, 8, 'dontdelmepls', 666),
(19, 8, 'delmeifyoudare', 44),
(20, 8, 'justanotherone', 11),
(22, 10, 'canyoudel', 22),
(23, 10, 'betyoucan', 45),
(24, 10, 'morechars', 999),
(25, 10, 'highlevel', 66777),
(28, 13, 'uusiukko', 21),
(29, 13, 'soturi', 55),
(30, 7, 'turhahahmo', 37),
(31, 7, 'mostuseless', 87),
(32, 7, 'fillerContent', 3),
(33, 7, 'boringname', 444),
(34, 11, 'needsmorecowbell', 97),
(35, 13, 'outofideas', 23),
(36, 14, 'noadminprivilege', 88888),
(37, 14, 'noexploitshere', 563724),
(38, 15, 'höpötiitti', 69);

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
  MODIFY `id` int(16) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `characters`
--
ALTER TABLE `characters`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

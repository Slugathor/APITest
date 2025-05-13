<?php
// handles connecting to MySQL database using PHP Data Objects (PDO)
$pdo = new PDO("mysql:host=localhost;dbname=testdb", "root", "");
$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
?>
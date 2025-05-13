<?php
header("Cache-Control: no-cache, no-store, must-revalidate");

session_start();
session_unset(); // clears session variables like username, id...
session_destroy(); // destroys the complete session

echo json_encode(["success" => true, "message" => "Logged out successfully."]);
?>

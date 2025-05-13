<?php
require_once 'db.php';
session_start();
$min_length = 1;

// Check if user is logged in
if (!isset($_SESSION['username'])) {
    http_response_code(401); // Unauthorized
    echo json_encode(["error" => "Not logged in"]);
    exit();
}

// Validate input
$data = json_decode(file_get_contents("php://input"), true);
if (!isset($data['new_password']) || strlen($data['new_password']) < $min_length) {
    http_response_code(400); // Bad Request
    echo json_encode(["error" => "Password must be at least 1 character long."]);
    exit();
}

$username = $_SESSION['username'];
$new_password = $data['new_password'] ?? null;
$hashed_password = password_hash($new_password, PASSWORD_DEFAULT);

try {
    $stmt = $pdo->prepare("UPDATE accounts SET password_hash = ? WHERE username = ?");
    $stmt->execute([$hashed_password, $username]);

    echo json_encode(["success" => true, "message" => "Password changed successfully."]);
} catch (PDOException $e) {
    http_response_code(500);
    echo json_encode(["error" => "Database error", "details" => $e->getMessage()]);
}
?>

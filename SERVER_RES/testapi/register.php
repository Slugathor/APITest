<?php
// REST API endpoint to register a new user

// Include database connection
require_once 'db.php';

// Read raw input and decode JSON
$input = json_decode(file_get_contents('php://input'), true);

// Check if username and password are provided
if (!isset($input['username']) || !isset($input['password'])) {
    http_response_code(400);
    echo json_encode(["message" => "Username and password required."]);
    exit;
}

$username = $input['username'];
$password = $input['password'];

// Check if the username already exists
$stmt = $pdo->prepare("SELECT id FROM accounts WHERE username = ?");
$stmt->execute([$username]);

if ($stmt->fetch()) {
    http_response_code(409); // Conflict
    echo json_encode(["message" => "Username already exists."]);
    exit;
}

// Hash the password before storing it
$hash = password_hash($password, PASSWORD_DEFAULT);

// Insert new user into database
$stmt = $pdo->prepare("INSERT INTO accounts (username, password_hash) VALUES (?, ?)");
$stmt->execute([$username, $hash]);

echo json_encode(["message" => "User registered successfully."]);
?>
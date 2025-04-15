<?php
//REST API endpoint that handles logging in with a username and password

session_start();
require_once "db.php";

$data = json_decode(file_get_contents("php://input"), true);

$username = $data['username'];
$password = $data['password'];

$stmt = $pdo->prepare("SELECT * FROM accounts WHERE username = ?");
$stmt->execute([$username]);
$user = $stmt->fetch(PDO::FETCH_ASSOC);

if ($user && password_verify($password, $user['password_hash'])) {
    $_SESSION['user_id'] = $user['id'];
    $_SESSION['username'] = $user['username'];
    echo json_encode(["message" => "Login successful"]);
} else {
    http_response_code(401);
    echo json_encode(["message" => "Invalid username or password"]);
}
?>
<?php
session_start();
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "testdb";
$conn = new mysqli($servername, $username, $password, $dbname);


if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

if (!isset($_SESSION['user_id'])) {
    http_response_code(401);
    echo json_encode(["error" => "Unauthorized"]);
    exit;
}

$uri = $_SERVER['REQUEST_URI'];
$method = $_SERVER['REQUEST_METHOD'];

if ($method == 'GET' && $uri == '/testapi/accounts') {
    $query = "SELECT id, username, accountType FROM accounts A WHERE A.id BETWEEN 1 AND 100;";
    $result = $conn->query($query);
    $accounts = $result->fetch_all(MYSQLI_ASSOC);
    echo json_encode($accounts);
}
elseif ($method == 'GET' && preg_match('/\/testapi\/characters\/(\d+)/', $uri, $matches)) {
    $accID = intval($matches[1]);
    $query = "SELECT * FROM characters C WHERE C.account_id = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("i", $accID);
    $stmt->execute();
    $result = $stmt->get_result();

    $characters = [];
    while ($row = $result->fetch_assoc()) {
        $characters[] = $row;
    }

    echo json_encode($characters);
}
elseif ($method == 'DELETE' && preg_match('/\/testapi\/characters\/delete\/(\d+)\/([A-Za-z0-9_]+)/', $uri, $matches)) {
    $accID = intval($matches[1]);
    $charName = urldecode($matches[2]);

    // Authorization check
    $currentUser = $_SESSION['user_id'];
    $accountType = $_SESSION['accountType'];
    // if not logged in as admin and trying to delete a character that does not belong to you
    if ($accountType != 1 && $currentUser != $accID) {
        http_response_code(403);
        echo json_encode(["error" => "Forbidden: You cannot delete characters from this account."]);
        exit;
    }

    $query = "DELETE FROM characters WHERE account_id = ? AND name = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("is", $accID, $charName);
    $stmt->execute();

    if ($stmt->affected_rows > 0) {
        echo json_encode(["success" => "Character '$charName' deleted."]);
    } else {
        http_response_code(404);
        echo json_encode(["error" => "Character not found or does not belong to the account."]);
    }
}

else {
    echo json_encode(["error" => "Invalid request"]);
}

$conn->close();
?>

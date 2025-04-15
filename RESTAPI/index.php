<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "testdb";
// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$uri = $_SERVER['REQUEST_URI'];
$method = $_SERVER['REQUEST_METHOD'];
switch ($method | $uri) {
    case($method =='GET' && $uri == '/testapi/accounts'):
        $query = "SELECT * FROM accounts A WHERE A.id BETWEEN 1 AND 3;";
        $result = $conn->query($query);
        $accounts = $result->fetch_all(MYSQLI_ASSOC);
        echo json_encode($accounts);
        break;

    case($method =='GET' && preg_match('/\/testapi\/characters\/account_id\/(\d+)/', $uri, $matches)):
        $accID = intval($matches[1]); // Extract account_id from URI
        $query = "SELECT username, level FROM characters C WHERE C.account_id = ?";

        $stmt = $conn->prepare($query);
        $stmt->bind_param("i", $accID);
        $stmt->execute();
        $result = $stmt->get_result();

        $characters = [];
        while ($row = $result->fetch_assoc()) {
            $characters[] = $row;
        }
        
        echo json_encode($characters);
        break;


        default:
        echo json_encode(["error" => "Invalid request"]);
        break;
}
$conn->close();
?>
<?php
// REST API endpoint that checks if valid session exists
// Respond with user-specific data if valid, else return error

session_start();
//echo "Session ID: " . session_id() . "\n";
if (!isset($_SESSION['user_id'])) {
    http_response_code(401);
    echo json_encode(["message" => "Not logged in"]);
    exit;
}

// user is logged in, so we can safely show data
echo "Logged in as {$_SESSION['username']}\n";
?>
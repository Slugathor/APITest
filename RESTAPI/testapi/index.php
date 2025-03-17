<?php
$uri = $_SERVER['REQUEST_URI'];
$method = $_SERVER['REQUEST_METHOD'];
switch ($method | $uri) {
    case($method =='GET' && $uri == '/testapi/accounts'):
        $query = "SELECT * FROM accounts A WHERE A.id BETWEEN 1 AND 3;";
        $result = $conn->query($query);
        //encode to json???
    case($method =='GET' && $uri == '/testapi/characters/byid/'): // how to muuttuja as parameter?!?!?
        $accID;
        $query = "SELECT * FROM characters C WHERE C.account_id = $accID"
}
?>

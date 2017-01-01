<?php
  session_start();
?>
<?php
  $path_admins = "Database/admins.database";

  $json_admins = json_decode(file_get_contents($path_admins), true);

  $session_key = (isset($_SESSION["key"]) ? $_SESSION["key"] : null);
  $session_username = (isset($_SESSION["username"]) ? $_SESSION["username"] : null);
  $username = (isset($_POST["username"]) ? $_POST["username"] : null);
  $password = (isset($_POST["password"]) ? $_POST["password"] : null);
  $IP = $_SERVER["REMOTE_ADDR"];
?>
<?php
  function reload(){
    header("Location: login.php");
    die();
  }

  function unlock(){
    header("Location: home.php");
    die();
  }

  function randomString($_len){
    $_ret = "";

    for($_i = 0; $_i < $_len; $_i++){
      $_sel = rand(1, 3);

      if($_sel == 1)
        $_ret .= chr(rand(48, 57));
      elseif($_sel == 2)
        $_ret .= chr(rand(65, 90));
      elseif($_sel == 3)
        $_ret .= chr(rand(97, 122));
    }

    return $_ret;
  }
?>
<?php
  if($session_key && $session_username){
    if(array_key_exists($session_username, $json_admins))
      if($json_admins[$session_username]["session"] == $session_key)
        if($json_admins[$session_username]["lastonline"] != null && (time() - $json_admins[$session_username]["lastonline"]) < 600)
          unlock();

    unset($_SESSION["key"]);
    unset($_SESSION["username"]);
    reload();
  }
  elseif($username && $password){
    if(!array_key_exists($username, $json_admins))
      reload();
    if(!$json_admins[$username]["password"]){
      $_salt = randomString(16);

      $json_admins[$username]["password"] = hash("sha256", $password . $_salt);
      $json_admins[$username]["salt"] = $_salt;
    }
    if(hash("sha256", $password . $json_admins[$username]["salt"]) != $json_admins[$username]["password"])
      reload();

    $_rStr = randomString(32);
    $json_admins[$username]["lastonline"] = time();
    $json_admins[$username]["IP"] = $IP;
    $json_admins[$username]["session"] = $_rStr;
    $_SESSION["key"] = $_rStr;
    $_SESSION["username"] = $username;

    file_put_contents($path_admins, json_encode($json_admins));
    unlock();
  }
?>
<html>
  <head>
    <title>ManPAD Login Page</title>
  </head>
  <body>
    <form action="login.php" method="post">
      Username: <input type="text" name="username" /><br />
      <br />
      Password: <input type="password" name="password" /><br />
      <br />
      <input type="submit" name="login" value="Login" />
    </form>
  </body>
</html>

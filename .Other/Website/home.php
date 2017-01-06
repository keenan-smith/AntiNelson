<?php
  session_start();
?>
<?php
  $path_admins = "Database/admins.database";
  $path_users = "Database/users.database";

  $json_admins = json_decode(file_get_contents($path_admins), true);

  $session_key = (isset($_SESSION["key"]) ? $_SESSION["key"] : null);
  $username = (isset($_SESSION["username"]) ? $_SESSION["username"] : null);
?>
<?php
  function lock(){
    header("Location: login.php");
    die();
  }

  function refresh(){
    header("Location: home.php");
    die();
  }
?>
<?php
  if(!$session_key || !$username)
    lock();
  if($json_admins[$username]["session"] != $session_key)
    lock();
  if($json_admins[$username]["lastonline"] == null || (time() - $json_admins[$username]["lastonline"]) >= 600)
    lock();

  $json_admins[$username]["lastonline"] = time();
?>
<?php
  if(isset($_POST["btn_Logout"])){
    unset($_SESSION["username"]);
    unset($_SESSION["key"]);
    lock();
  }
  if(isset($_POST["btn_Hacks"])){
    header("Location: hacks.php");
    die();
  }
  if(isset($_POST["btn_Users"])){
    header("Location: users.php");
    die();
  }
  if(isset($_POST["btn_DLUsers"])){
    header('Content-Type: application/octet-stream');
    header("Content-Transfer-Encoding: Binary");
    header("Content-disposition: attachment; filename=\"" . basename($path_users) . "\"");
    header("Content-Length: " . filesize($path_users));
    readfile($path_users);
    die();
  }
?>
<html>
  <head>
    <title>ManPAD Home Page</title>
  </head>
  <body>
    Welcome back <?php echo($username) ?> you sexy beast.<br /><br />
    <form action="home.php" method="post">
      <input type="submit" name="btn_Users" value="User Query" /><br />
      <br />
      <input type="submit" name="btn_Hacks" value="Hack Manager" /><br />
      <br />
      <input type="submit" name="btn_DLUsers" value="Download Users" /><br />
      <br />
      <input type="submit" name="btn_Logout" value="Logout" /><br />
    </form>
  </body>
</html>

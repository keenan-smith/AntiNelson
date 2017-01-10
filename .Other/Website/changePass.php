<?php
  session_start();
?>
<?php
  $path_admins = "Database/admins.database";

  $json_admins = json_decode(file_get_contents($path_admins), true);

  $session_key = (isset($_SESSION["key"]) ? $_SESSION["key"] : null);
  $username = (isset($_SESSION["username"]) ? $_SESSION["username"] : null);
?>
<?php
  function lock(){
    header("Location: login.php");
    die();
  }

  function home(){
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
  if(!$session_key || !$username)
    lock();
  if($json_admins[$username]["session"] != $session_key)
    lock();
  if($json_admins[$username]["lastonline"] == null || (time() - $json_admins[$username]["lastonline"]) >= 600)
    lock();

  $json_admins[$username]["lastonline"] = time();
  file_put_contents($path_admins, json_encode($json_admins));
?>
<?php
  if(isset($_POST["btn_Home"]))
    home();

  if(isset($_POST["btn_Save"])){
    if(isset($_POST["chk_resalt"]))
      $_salt = randomString(16);
    else
      $_salt = $json_admins[$username]["salt"];

    $json_admins[$username]["password"] = hash("sha256", $_POST["txt_password"] . $_salt);
    if(isset($_POST["chk_resalt"]))
      $json_admins[$username]["salt"] = $_salt;

    file_put_contents($path_admins, json_encode($json_admins));
    home();
  }
?>
<html>
  <head>
    <title>ManPAD Password Change</title>
  </head>
  <body>
    <form action="changePass.php" method="post">
      New Password: <input type="password" name="txt_password" />
      <br /><br />
      <input type="checkbox" name="chk_resalt" checked />Generate New Salt
      <br /><br />
      <input type="submit" name="btn_Save" value="Change Password" />
      <input type="submit" name="btn_Home" value="Go Home" />
    </form>
  </body>
</html>

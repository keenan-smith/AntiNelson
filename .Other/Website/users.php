<?php
  session_start();
?>
<?php
  $path_admins = "Database/admins.database";
  $path_users = "Database/users.database";

  $json_admins = json_decode(file_get_contents($path_admins), true);
  $json_users = json_decode(file_get_contents($path_users), true);

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

  function str_contains($_search, $_text){
    return (strpos($_text, $_search) !== false);
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
  if(isset($_POST["btn_back"]))
    home();
?>
<html>
  <head>
    <title>ManPAD User Query</title>
    <style>
      table {
        font-family: arial, sans-serif;
        border-collapse: collapse;
        width: 100%;
      }

      td, th {
        border: 1px solid #dddddd;
        text-align: left;
        padding: 8px;
      }

      tr:nth-child(even) {
        background-color: #dddddd;
      }
    </style>
  </head>
  <body>
    <form action="users.php" method="post">
      HWID: <input type="text" name="txt_HWID" <?php echo((!empty($_POST["txt_HWID"]) ? "value='" . $_POST["txt_HWID"] . "'" : "")) ?> />
      IP: <input type="text" name="txt_IP" <?php echo((!empty($_POST["txt_IP"]) ? "value='" . $_POST["txt_IP"] . "'" : "")) ?> />
      <br /><br />
      Username: <input type="text" name="txt_Username" <?php echo((!empty($_POST["txt_Username"]) ? "value='" . $_POST["txt_Username"] . "'" : "")) ?> />
      Steam 64: <input type="text" name="txt_Steam64" <?php echo((!empty($_POST["txt_Steam64"]) ? "value='" . $_POST["txt_Steam64"] . "'" : "")) ?> />
      <br /><br />
      <input type="submit" name="btn_query" value="Query Users" />
      <input type="submit" name="btn_clear" value="Clear Query" />
      <input type="submit" name="btn_back" value="Go Home" />
      <input type="checkbox" name="chk_Premium" <?php echo((!empty($_POST["chk_Premium"]) ? "checked" : "")) ?> />Premium Only
      <input type="checkbox" name="chk_Blacklist" <?php echo((!empty($_POST["chk_Blacklist"]) ? "checked" : "")) ?> />Blacklisted Only
      <hr />
    </form>
    <table>
      <tr>
        <th>Steam Name</th>
        <th>Steam 64</th>
        <th>IP</th>
        <th>HWID</th>
        <th>Premium</th>
        <th>Blacklisted</th>
        <th>Edit</th>
      </tr>
      <?php
        foreach($json_users as $_key => $_data){
          if(isset($_POST["btn_query"])){
            if((!empty($_POST["txt_HWID"]) ? !str_contains($_POST["txt_HWID"], $_key) : false))
              continue;
            if((!empty($_POST["txt_IP"]) ? !str_contains($_POST["txt_IP"], $_data["IP"]) : false))
              continue;
            if((!empty($_POST["txt_Username"]) ? !str_contains($_POST["txt_Username"], $_data["steam_name"]) : false))
              continue;
            if((!empty($_POST["txt_Steam64"]) ? !str_contains($_POST["txt_Steam64"], $_data["steam_64"]) : false))
              continue;
            if(!empty($_POST["chk_Premium"]) && !$_data["isPremium"])
              continue;
            if(!empty($_POST["chk_Blacklist"]) && !$_data["isBlacklisted"])
              continue;
          }

          echo("<tr>");
          echo("<td>" . urldecode($_data["steam_name"]) . "</td>");
          echo("<td>" . $_data["steam_64"] . "</td>");
          echo("<td>" . $_data["IP"] . "</td>");
          echo("<td>" . $_key . "</td>");
          echo("<td>" . ($_data["isPremium"] ? "Yes" : "No") . "</td>");
          echo("<td>" . ($_data["isBlacklisted"] ? "Yes" : "No") . "</td>");
          echo("<td>");
          echo("<form action='user.php' method='post'>");
          echo("<input type='hidden' name='HWID' value='" . $_key . "' />");
          echo("<input type='submit' name='btn_Edit' value='Edit' />");
          echo("</form>");
          echo("</td>");
          echo("</tr>");
        }
      ?>
    </table>
  </body>
</html>

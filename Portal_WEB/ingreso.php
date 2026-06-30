<?php

// Mostrar errores (solo para pruebas)
error_reporting(E_ALL);
ini_set('display_errors', 1);

session_start();

$conexion = new mysqli("localhost", "root", "", "mi_banco_db");

if ($conexion->connect_error)
{
    die("Error de conexión: " . $conexion->connect_error);
}

$tipo_doc = $_POST["tipo_doc"];
$documento = $_POST["documento"];
$usuario = $_POST["usuario"];
$password = $_POST["password"];

// Buscar el usuario
$sql = "SELECT * FROM usuarios
        WHERE documento = ?
        AND tipo_doc = ?
        AND usuario = ?
        AND password = ?";

$stmt = $conexion->prepare($sql);

if (!$stmt)
{
    die("Error en la consulta: " . $conexion->error);
}

$stmt->bind_param(
    "ssss",
    $documento,
    $tipo_doc,
    $usuario,
    $password
);

$stmt->execute();

$resultado = $stmt->get_result();

if ($resultado->num_rows == 1)
{
    $fila = $resultado->fetch_assoc();

    $_SESSION["documento"] = $fila["documento"];
    $_SESSION["usuario"] = $fila["usuario"];
    $_SESSION["nombre"] = $fila["nombre"];
    $_SESSION["apellido"] = $fila["apellido"];

    header("Location: resumen.php");
    exit();
}
else
{
    echo "<h2>Datos incorrectos.</h2>";
}

$stmt->close();
$conexion->close();

?>
<?php

$conexion = new mysqli("localhost", "root", "", "mi_banco_db");

if ($conexion->connect_error) {
    die("Error de conexión: " . $conexion->connect_error);
}

$tipo_doc = $_POST["tipo_doc"];
$documento = $_POST["documento"];
$nombre = $_POST["nombre"];
$apellido = $_POST["apellido"];
$fecha_nacimiento = $_POST["fecha_nacimiento"];
$email = $_POST["email"];
$usuario = $_POST["usuario"];
$passwordA = $_POST["passwordA"];
$passwordB = $_POST["passwordB"];

if ($passwordA != $passwordB)
{
    die("Las contraseñas no coinciden.");
}

// 4) Validar tipo de documento
if ($tipo_doc != "DNI" && $tipo_doc != "PASAPORTE")
{
    die("Tipo de documento inválido.");
}

// 5) Verificar que el cliente exista y tenga una tarjeta asociada
$sql = "SELECT * 
        FROM usuarios u
        INNER JOIN tarjetas t 
        ON u.documento = t.dni_titular
        WHERE u.documento = ?
        AND u.tipo_doc = ?
        AND u.nombre = ?
        AND u.apellido = ?
        AND u.fecha_nacimiento = ?
        AND u.email = ?";

$stmt = $conexion->prepare($sql);

$stmt->bind_param(
    "ssssss",
    $documento,
    $tipo_doc,
    $nombre,
    $apellido,
    $fecha_nacimiento,
    $email
);

$stmt->execute();

$resultado = $stmt->get_result();

if ($resultado->num_rows == 0)
{
    die("No se encontró una tarjeta asociada a esos datos.");
}

// 6) Activar la cuenta web actualizando usuario y contraseña
$sqlUpdate = "UPDATE usuarios
              SET usuario = ?, password = ?
              WHERE documento = ?";

$stmtUpdate = $conexion->prepare($sqlUpdate);

$stmtUpdate->bind_param(
    "sss",
    $usuario,
    $passwordA,
    $documento
);

if ($stmtUpdate->execute())
{
    echo "<h2>Cuenta activada correctamente.</h2>";
    echo "<a href='ingreso.html'>Ir al inicio de sesión</a>";
}
else
{
    echo "Error al activar la cuenta.";
}

// Cerrar la conexión
$conexion->close();
?>
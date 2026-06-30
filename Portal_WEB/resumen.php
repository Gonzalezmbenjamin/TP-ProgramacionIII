<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

session_start();

if (!isset($_SESSION["documento"]))
{
    header("Location: ingreso.html");
    exit();
}

$conexion = new mysqli("localhost", "root", "", "mi_banco_db");

if ($conexion->connect_error)
{
    die("Error de conexión: " . $conexion->connect_error);
}

$documento = $_SESSION["documento"];
$nombre = $_SESSION["nombre"];
$apellido = $_SESSION["apellido"];

echo "<h1>Bienvenido, $nombre $apellido</h1>";
echo "<p><strong>Documento:</strong> $documento</p>";

// 1) Buscar última liquidación
$sqlActual = "SELECT
                l.periodo,
                l.fecha_vencimiento,
                l.total_a_pagar,
                l.pago_minimo
              FROM usuarios u
              INNER JOIN tarjetas t
                ON u.documento = t.dni_titular
              INNER JOIN liquidaciones l
                ON t.num_cuenta = l.num_cuenta
              WHERE u.documento = ?
              ORDER BY l.periodo DESC
              LIMIT 1";

$stmtActual = $conexion->prepare($sqlActual);
$stmtActual->bind_param("s", $documento);
$stmtActual->execute();
$resultadoActual = $stmtActual->get_result();

echo "<h2>Liquidación Actual</h2>";

if ($resultadoActual->num_rows == 1)
{
    $actual = $resultadoActual->fetch_assoc();

    echo "<div style='border:1px solid #999; padding:15px; width:400px; margin-bottom:25px;'>";
    echo "<p><strong>Período:</strong> " . $actual["periodo"] . "</p>";
    echo "<p><strong>Fecha de vencimiento:</strong> " . $actual["fecha_vencimiento"] . "</p>";
    echo "<p><strong>Total a pagar:</strong> $ " . $actual["total_a_pagar"] . "</p>";
    echo "<p><strong>Pago mínimo:</strong> $ " . $actual["pago_minimo"] . "</p>";
    echo "</div>";
}
else
{
    echo "<p>No hay liquidaciones disponibles.</p>";
}

// 2) Buscar historial sin incluir la última
$sqlHistorial = "SELECT
                    l.periodo,
                    l.fecha_vencimiento,
                    l.total_a_pagar,
                    l.pago_minimo
                 FROM usuarios u
                 INNER JOIN tarjetas t
                    ON u.documento = t.dni_titular
                 INNER JOIN liquidaciones l
                    ON t.num_cuenta = l.num_cuenta
                 WHERE u.documento = ?
                 AND l.periodo < (
                    SELECT MAX(l2.periodo)
                    FROM liquidaciones l2
                    INNER JOIN tarjetas t2
                        ON l2.num_cuenta = t2.num_cuenta
                    WHERE t2.dni_titular = ?
                 )
                 ORDER BY l.periodo DESC";

$stmtHistorial = $conexion->prepare($sqlHistorial);
$stmtHistorial->bind_param("ss", $documento, $documento);
$stmtHistorial->execute();
$resultadoHistorial = $stmtHistorial->get_result();

echo "<h2>Historial de Liquidaciones</h2>";

if ($resultadoHistorial->num_rows > 0)
{
    echo "<table border='1' cellpadding='8' cellspacing='0'>";
    echo "<tr>";
    echo "<th>Período</th>";
    echo "<th>Fecha de vencimiento</th>";
    echo "<th>Total a pagar</th>";
    echo "<th>Pago mínimo</th>";
    echo "</tr>";

    while ($fila = $resultadoHistorial->fetch_assoc())
    {
        echo "<tr>";
        echo "<td>" . $fila["periodo"] . "</td>";
        echo "<td>" . $fila["fecha_vencimiento"] . "</td>";
        echo "<td>$ " . $fila["total_a_pagar"] . "</td>";
        echo "<td>$ " . $fila["pago_minimo"] . "</td>";
        echo "</tr>";
    }

    echo "</table>";
}
else
{
    echo "<p>No hay liquidaciones anteriores.</p>";
}

$stmtActual->close();
$stmtHistorial->close();
$conexion->close();

?>
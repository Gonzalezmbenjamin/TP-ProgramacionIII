# TP Programación III - Progra3Card

## Integrantes

- Marcos Benjamín González
- Lautaro Pared

---

## Descripción

Progra3Card es un sistema de administración y consulta de tarjetas bancarias compuesto por dos módulos:

### Aplicación de Consola (C#)

Permite:

- Emitir una nueva tarjeta.
- Listar todas las tarjetas.
- Consultar el detalle de una tarjeta y su titular.
- Eliminar una tarjeta.
- Emitir una nueva liquidación mensual.

### Portal Web (PHP)

Permite:

- Activar una cuenta web.
- Iniciar sesión.
- Consultar la liquidación actual.
- Consultar el historial de liquidaciones.

---

## Tecnologías utilizadas

- C#
- .NET 8
- PHP
- MySQL
- XAMPP
- HTML
- Tailwind CSS

---

## Base de datos

La base de datos utilizada es:

`mi_banco_db`

El script para crearla se encuentra en:

`mi_banco_db.sql`

---

## Ejecución del proyecto

### Aplicación de Consola

Abrir la carpeta **Aplicacion_Consola** y ejecutar:

```bash
dotnet run
```

### Portal Web

1. Copiar la carpeta **Portal_WEB** dentro de:

```
C:\xampp\htdocs\
```

2. Iniciar **Apache** y **MySQL** desde XAMPP.

3. Abrir en el navegador:

```
http://localhost/Portal_WEB/ingreso.html
```

o

```
http://localhost/Portal_WEB/registro.html
```

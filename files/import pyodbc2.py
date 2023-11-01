import pyodbc
import csv

# Conexión a la base de datos SQL Server
server = 'localhost'  # Reemplaza con el nombre de tu servidor SQL
database = 'okta_db'  # Reemplaza con el nombre de tu base de datos
username = 'sa'  # Reemplaza con tu nombre de usuario
password = 'admin'  # Reemplaza con tu contraseña
connection_string = f'DRIVER={{SQL Server}};SERVER={server};DATABASE={database};UID={username};PWD={password}'

conn = pyodbc.connect(connection_string)
cursor = conn.cursor()

# Ruta al archivo CSV
csv_file_path = 'MOCK_DATA.csv'  # Reemplaza con la ruta a tu archivo CSV

# Nombre de la tabla de destino en la base de datos
table_name = 'userapp'  # Reemplaza con el nombre de tu tabla

# Abre el archivo CSV y realiza la inserción
try:
    with open(csv_file_path, 'r', newline='', encoding='utf-8') as csvfile:
        csvreader = csv.reader(csvfile)
        next(csvreader)  # Salta la primera fila si es un encabezado

        for row in csvreader:
            # Suponemos que las dos primeras columnas son id y name
            id_value, name_value, other_value, other_value2, other_value3, other_value4, other_value5, other_value6 = row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7]
            insert_query = f"INSERT INTO {table_name} (FirstName, LastName, Email, DateBirth, Age, PasswordHash, SecurityStamp, ConcurrencyStamp) VALUES (?, ?, ?, ?, ?, ?, ?, ?)"
            cursor.execute(insert_query, id_value, name_value, other_value, other_value2, other_value3, other_value4, other_value5, other_value6)

    conn.commit()
    print("Inserción completada correctamente.")
except Exception as e:
    conn.rollback()
    print(f"Error al insertar datos: {str(e)}")
finally:
    conn.close()

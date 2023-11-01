import pyodbc
import csv

# Conexión a la base de datos SQL Server
server = 'tcp:araujo.database.windows.net'  # Reemplaza con el nombre de tu servidor SQL
database = 'learning'  # Reemplaza con el nombre de tu base de datos
username = 'learning'  # Reemplaza con tu nombre de usuario
password = 'Br!@nf0x2024'  # Reemplaza con tu contraseña
connection_string = f'DRIVER={{SQL Server}};SERVER={server};DATABASE={database};UID={username};PWD={password}'

conn = pyodbc.connect(connection_string)
cursor = conn.cursor()

# Ruta al archivo CSV
csv_file_path = 'countries.csv'  # Reemplaza con la ruta a tu archivo CSV

# Nombre de la tabla de destino en la base de datos
table_name = 'country'  # Reemplaza con el nombre de tu tabla

# Abre el archivo CSV y realiza la inserción
try:
    with open(csv_file_path, 'r', newline='', encoding='utf-8') as csvfile:
        csvreader = csv.reader(csvfile)
        next(csvreader)  # Salta la primera fila si es un encabezado

        for row in csvreader:
            # Suponemos que las dos primeras columnas son id y name
            id_value, name_value, value_2 = row[0], row[1], row[2]
            insert_query = f"INSERT INTO {table_name} (id, name, ) VALUES (?, ?)"
            cursor.execute(insert_query, id_value, name_value)

    conn.commit()
    print("Inserción completada correctamente.")
except Exception as e:
    conn.rollback()
    print(f"Error al insertar datos: {str(e)}")
finally:
    conn.close()

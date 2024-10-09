# RealEstate EDA Solution

Este proyecto implementa una arquitectura de **Event-Driven Architecture (EDA)** utilizando una combinación de microservicios y comunicación basada en eventos para manejar los diferentes componentes del sistema de manera desacoplada y eficiente.

En la solución se encuemtran:

- 2 APIs:
  - API Producer: Para registrar los eventos en kafka.
  - API Report: Para obtener la infomación en base de datos)
- 1 Servicio (Consumer)

## Estructura de la solución

La solución fue creada en **Microsoft Visual Studio**, siguiendo un enfoque modular. Se estructuró utilizando diferentes proyectos que permiten la separación de responsabilidades y mejorar la mantenibilidad del código. Aquí una descripción general de los proyectos:

1. **RealEstate.Producer**: 
   Este proyecto se encarga de producir eventos a Kafka. La lógica de negocio relevante para la publicación de eventos, como nuevas propiedades o actualizaciones en el sistema, es manejada aquí.

2. **RealEstate.Service**: 
  Este proyecto consume los eventos desde Kafka y los procesa. El objetivo principal es garantizar que las acciones que dependen de estos eventos se ejecuten correctamente, como la actualización de bases de datos o la notificación a otros sistemas.

3. **RealEstate.API**: 
   Este proyecto expone los endpoints necesarios para interactuar con el sistema, permitiendo a los usuarios consultar la información de propiedades.

4. **RealEstate.Database**: 
   Este proyecto maneja la conexión y acceso a la base de datos. Toda la lógica de persistencia está centralizada aquí para asegurar una correcta gestión de la información del sistema.

5. **RealEstate.Shared**: 
   Contiene las entidades y modelos compartidos entre los proyectos, asegurando consistencia en la definición de los datos a lo largo de toda la solución.

6. **RealEstate.Application**: 
   Este proyecto centraliza la lógica de aplicación, coordinando las interacciones entre los diferentes servicios.

## Justificación de la Arquitectura EDA

Yo decidí implementar una arquitectura basada en eventos (EDA) por varias razones:

1. **Desacoplamiento**: Utilizando eventos para comunicar los diferentes componentes, se puede asegurar que cada parte del sistema funcione de manera independiente. Por ejemplo, el productor de eventos no necesita conocer los detalles del consumidor. Esto mejora la escalabilidad y facilita futuras modificaciones sin afectar a todo el sistema.

2. **Escalabilidad**: Con Kafka como intermediario de mensajes, el sistema puede manejar grandes volúmenes de eventos de manera eficiente, y permite la adición de nuevos consumidores o productores sin necesidad de modificar los componentes existentes.

3. **Resiliencia**: Al procesar eventos de forma asincrónica, el sistema es capaz de continuar funcionando incluso cuando algunas partes estén temporalmente inactivas. Los eventos que no puedan ser procesados de inmediato pueden ser almacenados y reintentados más tarde, asegurando que las acciones importantes no se pierdan.

## Docker Compose

Para facilitar la ejecución y pruebas del sistema, se ha configurado un archivo `docker-compose.yml` que define los servicios necesarios para la arquitectura de EDA. Aquí están los servicios principales y su configuración:

- **Zookeeper**: Es el coordinador que gestiona los brokers de Kafka. Se utilizó la imagen de Zookeeper de Bitnami y se habilitó el acceso anónimo para simplificar el entorno de desarrollo.

- **Kafka**: Este servicio es el corazón del sistema de mensajería. Yo decidí utilizar Kafka porque es una solución robusta y probada para sistemas basados en eventos, lo que permite manejar grandes volúmenes de mensajes. Se configuraron puertos y listeners para permitir tanto la comunicación interna como externa.

- **Kafka UI**: Para facilitar el monitoreo y administración de los tópicos y eventos en Kafka, se incluyó un servicio de Kafka UI. Esto permite una vista gráfica del clúster Kafka, útil para depuración y verificación de la actividad de eventos.

### Ejecución del Docker Compose

Para levantar todos los servicios necesarios, simplemente se debe ejecutar el siguiente comando:

```bash
docker-compose up

```
### Evidencias

![image](https://github.com/user-attachments/assets/528a899d-882f-4473-a606-d01e11747cfa)

Interfaz para verificar broker.
![image](https://github.com/user-attachments/assets/19b5457f-546f-4a11-8ee5-4c7f1d19eec5)

Servicio (**RealEstate.Service**). Consumer
![image](https://github.com/user-attachments/assets/74c9bb45-fe09-45b4-8fa8-6df03966b22b)

API (**RealEstate.Producer**). Producer
![image](https://github.com/user-attachments/assets/978a539a-4346-448d-bef0-a173f93a3d42)

Llamado al API - Producer:
![image](https://github.com/user-attachments/assets/5e5116a1-31e1-496f-8a6e-bb821837e73f)

Mensaje (message) registrado en Kafka.
![image](https://github.com/user-attachments/assets/b0f8907b-afc3-4365-8ce3-62de7e33eb29)


Se activa el serivio, del consumer a través del topic **"property-consumer"**
![image](https://github.com/user-attachments/assets/1d679f5e-852a-4798-8408-0658fd3902b1)

Se consulta a la API - Report para obtener el registro:
![image](https://github.com/user-attachments/assets/68d03913-6945-406e-bc2b-b4bccaf9263c)
![image](https://github.com/user-attachments/assets/471b83aa-84d3-409b-8c84-1bfa9297810d)
![image](https://github.com/user-attachments/assets/54796b9a-c254-464c-8aa4-d760752f92a8)



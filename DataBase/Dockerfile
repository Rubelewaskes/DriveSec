FROM postgres:latest

# Копирование бекапа базы данных в контейнер
COPY DS_Backup.sql /docker-entrypoint-initdb.d/

# Указание переменных окружения для создания пользователя и пароля
ENV POSTGRES_PASSWORD=1234
ENV POSTGRES_DB=dbDS

# Указание портов, на которых будет работать контейнер
EXPOSE 5438

# Восстановление базы данных из бекапа
CMD ["postgres"]
# Journey Miles

# Criando usuário no banco de dados
- Acesse o mysql pelo terminal;
- Rode o comando: `CREATE USER 'journey-miles'@'localhost' IDENTIFIED BY 'SUA_SENHA';`;
- Definir privilégios para o usuário: `GRANT ALL PRIVILEGES ON * . * TO 'journey-miles'@'localhost';`;
- Aplicar privilégios: `FLUSH PRIVILEGES;`;
- Ref: https://www.hostinger.com/tutorials/mysql/how-create-mysql-user-and-grant-permissions-command-line

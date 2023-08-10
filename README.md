# Journey Miles (Jornada Milhas)

API que disponibiliza destinos de viagens e permite que usuários registrem um depoimento sobre um determinado local. Além disso, a API possuí integração com o ChatGPT que é utilizado para elaborar um texto descritivo sobre um destino.

# Criando usuário no banco de dados
- Acesse o mysql pelo terminal;
- Rode o comando: `CREATE USER 'NOME_USUARIO_BANCO_DE_DADOS'@'localhost' IDENTIFIED BY 'SUA_SENHA';`;
- Definir privilégios para o usuário: `GRANT ALL PRIVILEGES ON * . * TO 'NOME_USUARIO_BANCO_DE_DADOS'@'localhost';`;
- Aplicar privilégios: `FLUSH PRIVILEGES;`;
- Ref: https://www.hostinger.com/tutorials/mysql/how-create-mysql-user-and-grant-permissions-command-line

# Salvando dados sensíveis através do User-Secrets 
Será necessário definir três valores utilizando o user-secrets: GPTApiKey, GPTOrganizationKey e Mysql;
Primeiramente rode o comando: `dotnet user-secrets init --project=API`  
- GPTApiKey
  - A chave é gerada através do link: https://platform.openai.com/account/api-keys;
  - Utilize o comando `dotnet user-secrets set "OpenAI:GPTApiKey" "CHAVE_GERADA" --project=API`
- GPTOrganizationKey
  - Você pode obter através do link: https://platform.openai.com/account/org-settings
  - Utilize o comando `dotnet user-secrets set "OpenAI:GPTOrganizationKey" "ORGANIZACAO_ID" --project=API`
- Mysql
  - Rode o comando `dotnet user-secrets set "ConnectionStrings:Mysql" "Server=localhost;Port=3306;Database=NOME_DO_BANCO_DE_DADOS;User=NOME_USUARIO_BANCO_DE_DADOS;Password=SENHA_USUARIO_BANCO_DE_DADOS;" --project=API`
Ref: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=linux   

# Sugestão plugin para o VSCODE
- Se possível, utilize o plugin C# Dev Kit da Microsoft;
  - https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit

# Rodando a aplicação
- Rode o comando `dotnet run --project=API`;
  - Acesse a URL: http://localhost:5249/swagger/index.html
 

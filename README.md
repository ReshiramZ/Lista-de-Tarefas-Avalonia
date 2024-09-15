# Erros comuns:
## "Você não pode salvar uma lista se a mesma estiver vazia.":
Você está tentando criar uma lista, mas o conteúdo que você está tentando salvar está vazio, coloque conteúdo nela e tente novamente.
## "Não é possível sobrescrever uma lista com o conteúdo atual, pois o mesmo está vazio.":
Parecido com o anterior, o conteúdo da lista está vazio, Adiciona uma nova tarefa e tente novamente.
## "Para salvar rápido, precisasse colocar conteúdo e estar em uma lista já salva.":
Carregue uma lista já salva e coloque conteúdo nela, apertando no botão de carregar e adicionando novas tarefas.
## "O nome da lista é inválido, tente novamente.":
Você está tentando salvar uma lista com um nome inválido, geralmente nomes com: 
* acentos gráficos nas letras (Como 'Á' 'Ã' 'Â')
* números como primeiro caractere (Como '1', '4', '8')
* simbolos (Como '$', '@', '*') 
* ou simplesmente nomes que contém apenas palavras reservadas do banco de dados SQLite (Por exemplo, o nome 'DROP' não é permitido, mas 'TEAR DROP' é)
#### as palavras reservadas do SQLite são: 
* "SELECT"
* "INSERT"
* "UPDATE"
* "DELETE"
* "FROM"
* "WHERE"
* "JOIN"
* "CREATE"
* "DROP"
* "ALTER"
*  "TABLE"
* "DATABASE"
* "AND"
*  "OR"
* "NOT"
* "NULL"
* "LIKE"

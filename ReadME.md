# Minimal API
Este projeto modelo objetiva apresentar a implementação de minimal API's utilizando .NetCore 6. Nele utilizamos uma estrutura de banco de dados InMemory, ou seja, todos os dados armazenados e manipulados estão apenas no cache enquanto da execução da aplicação.

## Detalhes
### Resumo
Essa aplicação possui um conjunto de métodos REST para diversas operações assincronas, inclusive o consumo de um endpoint externo ppara retorno em tempo de execução, algo muito interessante em situações onde há dependencia de serviços externos, integrações ou mesmo o uso de microsserviços.

### Métodos
#### Get (/)
O método get principal retorna apenas uma sxtring de hello world.
#### Get (/frases)
Este método retorna uma string aleatória a partir de um endpoint externo, consultando e retornando o response desse endpoint.
#### Get (/tarefas)
Retorna uma lista contendo as tarefas cadastradas no Db (no nosso caso na memória).
#### Get (/tarefas/concluidas)
Retorna uma lista contendo apenas as tarefas cujo atributo de conclusão está como true.
#### Get (/tarefas/{id})
Retorna a tarefa cujo ID foi passado na request.
#### Put (/tarefas/{id})
Permite a edição de uma tarefa a partir do Id da mesma.
#### Delete (/tarefas/{id})
Permite a deleção de uma tarefa a partir do Id da mesma.
#### Post (/tarefas)
Permite a criação de uma tarefa.
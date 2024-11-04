Já existe um Usuário Adminsitrador cadastrado no Startup.cs. Suas credenciais são: Login: vet@clinvet.com e Senha: Senha@123  
Esse administrador é essencial para cadastrar todos os demais usuários no sistema. Existem Veterinarios, Secretarios, Proprietarios e Curiosos.  
Curiosos são criados através do login com o Google (não podem ser criados pelo administrador).  
Ao se logar como administrador, poderá inserir os demais atores.  
Veterinarios e Secretarios podem inserir Proprietarios.  
Somente Veterinarios podem inserir outros Veterinarios, Secretarios e Proprietarios.  
Veterinarios e Secretarios inserem os Pets.
Primeiro insere Proprietario. Depois insere os seus Pets.  
Tendo Pets e Veterinarios, podemos inserir os Encontros.  
Tendo um Encontro, podemos inserir Tratamentos que podem ter um ou muitos medicamentos atrelados.  
A opção Ver Tratamentos ao listar os Encontros permite cadastrar um novo Tratamento associado a um Pet e Veterinario que executa a ação.  
O sistema permite a exibição de Agenda. A Agenda pode ser editada por Veterinarios e Secretarios.  
Ao clicar em Minha Área você volta para a página específica ao seu Role.
A Agenda pode ser visualizada por Curiosos, Proprietarios e pessoas que não tem conta (pública). Eles não podem Editá-la.  
Apenas Veterinarios e Secretarios editam a Agenda. A Agenda pode ser vista por Veterinario ou podemos ver TODOS os agendamentos juntos.  
Os Proprietarios só podem visualizar os seus Pets. Eles podem visualizar os Encontros e Tratamentos. Porém não podem editar nada.  
Secretários não podem editar Encontros ou Tratamentos. Apenas Veterinarios podem fazer isso.  
Na página inicial uma pessoa não logada poderá acessar a lista de Veterinarios, de Secretarios e a Agenda.  
Ao clicar em ClinVet você será direcionado para a página Home do sistema.

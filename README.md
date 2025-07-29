## 🎯 O que é

**API RESTful** que simula as operações de uma plataforma de investimentos digital, além de uma interface **frontend** (web ou mobile) para interação com a API. O objetivo é representar o fluxo real de um banco de investimentos, incluindo movimentações financeiras, aplicações em diferentes tipos de ativos e a visualização dessas operações pelo usuário. O frontend deve permitir que os usuários criem contas, consultem saldos, realizem operações financeiras e acompanhem seus investimentos de forma intuitiva, proporcionando uma experiência próxima à de uma plataforma real de investimentos, inicialmente a proposta era para o Hackathon da OrangeJuice mas decidi utilizar do desafio como uma forma de aprender o framework .NET, além de conceitos como DDD e Clean Architecture, portanto estou desenvolvendo ela aos poucos, conciliando com trabalho CLT e estudos, mas tentando sempre trazer atualizações para uma consistência no aprendizado. 

**OBS:** a partir daqui eu mantive o mesmo pronunciado do desafio.

---

## 🧠 Contexto de Negócio

A FCamara atende bancos de investimentos e multiplos que possuem expressão a nível Global. Este desafio foi inspirado no domínio de negócio real que atuamos diariamente com nossos clientes, tornando esta uma oportunidade de exercitar habilidades técnicas em um cenário próximo da realidade.

---

## 📌 Funcionalidades Obrigatórias

Sua API deve conter funcionalidades que permitam:

- Criar contas de usuários
- Consultar saldo
- Realizar **depósitos**, **saques** e **transferências** entre contas
- Investir em ativos:
  - Renda variável (ações fictícias)
  - Fundos de investimento
  - Renda fixa:
    - CDB
    - Tesouro Direto
- Realizar **compra e venda de ativos**
- Cálculo automático de taxas e tributos por operação

---

## 💼 Regras de Negócio

As regras de negócio detalhadas para este desafio estão disponíveis no arquivo [`regradenegocio`](./regradenegocio) localizado neste mesmo diretório.  
Consulte esse arquivo para entender todos os requisitos e restrições que sua solução deve atender.

﻿namespace trybank;

public class Trybank
{
    public bool Logged;
    public int loggedUser;
    
    //0 -> Número da conta
    //1 -> Agência
    //2 -> Senha
    //3 -> Saldo
    public int[,] Bank;
    public int registeredAccounts;
    private int maxAccounts = 50;
    public Trybank()
    {
        loggedUser = -99;
        registeredAccounts = 0;
        Logged = false;
        Bank = new int[maxAccounts, 4];
    }

    private void IsThereLoggedUser()
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }
    }

    private void UserHasSufficientBalance(int value)
    {
        if (value > Bank[loggedUser, 3])
        {
            throw new InvalidOperationException("Saldo insuficiente");
        }
    }

    // 1. Construa a funcionalidade de cadastrar novas contas
    public void RegisterAccount(int number, int agency, int pass)
    {
        for(int i = 0; i < registeredAccounts; i++)
        {
            if (Bank[i, 0] == number && agency == Bank[i, 1])
            {
                throw new ArgumentException("A conta já está sendo usada!");
            }
        }
        Bank[registeredAccounts, 0] = number;
        Bank[registeredAccounts, 1] = agency;
        Bank[registeredAccounts, 2] = pass;
        Bank[registeredAccounts, 3] = 0;
        registeredAccounts ++;
    }

    // 2. Construa a funcionalidade de fazer Login
    public void Login(int number, int agency, int pass)
    {
        if (Logged)
        {
            throw new AccessViolationException("Usuário já está logado");
        }
        for (int i = 0; i < registeredAccounts; i ++)
        {
            if (number == Bank[i, 0] && agency == Bank[i, 1])
            {
                if (pass == Bank[i, 2])
                {
                    loggedUser = i;
                    Logged = true;
                    return;
                }
                else
                {
                    throw new ArgumentException("Senha incorreta");
                }
            }
            
        }
        throw new ArgumentException("Agência + Conta não encontrada");
    }

    // 3. Construa a funcionalidade de fazer Logout
    public void Logout()
    {
        IsThereLoggedUser();   
        Logged = false;
        loggedUser = -99;
    }

    // 4. Construa a funcionalidade de checar o saldo
    public int CheckBalance()
    {
        IsThereLoggedUser();
        return Bank[loggedUser, 3];
    }

    // 5. Construa a funcionalidade de depositar dinheiro
    public void Deposit(int value)
    {
        IsThereLoggedUser();
        Bank[loggedUser, 3] += value;
    }

    // 6. Construa a funcionalidade de sacar dinheiro
    public void Withdraw(int value)
    {
        IsThereLoggedUser();
        UserHasSufficientBalance(value);
        Bank[loggedUser, 3] -= value;
    }

    // 7. Construa a funcionalidade de transferir dinheiro entre contas
    public void Transfer(int destinationNumber, int destinationAgency, int value)
    {
        IsThereLoggedUser();
        UserHasSufficientBalance(value);
        for (int i = 0; i < registeredAccounts; i++)
        {
            if (Bank[i, 0] == destinationNumber && Bank[i, 1] == destinationAgency)
            {
                Bank[i, 3] += value;
                Withdraw(value);
            }
        }
    }

   
}

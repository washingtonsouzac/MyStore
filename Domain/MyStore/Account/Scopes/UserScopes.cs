using DomainNotificationHelper.Validation;
using MyStore.Domain.Account.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Domain.Account.Scopes
{
    public static class UserScopes
    {
        public static bool RegisterScopeIsValid(this User user)
        {
            return AssertionConcern.IsSatisfiedBy
                (
                    AssertionConcern.AssertLength(user.Username, 6, 20, "O usuário deve conter entre 6 e 20 caracteres!"),
                    AssertionConcern.AssertLength(user.Password, 6, 20, "A senha deve conter entre 6 e 20 caracteres!")
                );
        }

        public static bool VerificationScopeIsValid(this User user, string verificationCode)
        {
            return AssertionConcern.IsSatisfiedBy
                (
                    AssertionConcern.AssertNotNull(user, "Nenhum usuário encontrado para este aluno"), 
                    AssertionConcern.AssertTrue(user.Verified == false, "Aluno já verificado"),
                    AssertionConcern.AssertAreEquals(user.VerificationCode, verificationCode, "O código de verificação não confere")
                );
        }

        public static bool ActivationScopeIsValid(this User user, string activationCode)
        {
            return AssertionConcern.IsSatisfiedBy
                (
                    AssertionConcern.AssertNotNull(user, "Nenhum usuário encontrado para este aluno"),
                    AssertionConcern.AssertTrue(user.Verified == true, "E-mail não verificado"),
                    AssertionConcern.AssertTrue(user.Active == false, "Cadastro não ativado"),
                    AssertionConcern.AssertAreEquals(user.ActivationCode, activationCode, "Código de ativação não confere")
                );
        }

        public static bool RequestLoginScopeIsValid(this User user, string username)
        {
            return AssertionConcern.IsSatisfiedBy
                (
                    AssertionConcern.AssertNotNull(user, "Nenhum usuário encontrado para este aluno"),
                    AssertionConcern.AssertTrue(user.Verified == true, "E-mail não verificado"),
                    AssertionConcern.AssertTrue(user.Active == false, "Cadastro não ativado"),
                    AssertionConcern.AssertAreEquals(user.Username.ToLower(), username, "O nome do usuário não Confere"),
                    AssertionConcern.AssertAreEquals(DateTime.Compare(user.LastAuthorizationCodeRequest.AddMinutes(5), DateTime.Now).ToString(), (-1).ToString(), "Um SMS já foi enviado. Aguarde 5 minutos para requisitar um novo Login")
                );
        }

        public static bool LoginScopeIsValid(this User user, string authorizationCode, string password)
        {
            return AssertionConcern.IsSatisfiedBy
                (
                    AssertionConcern.AssertNotNull(user, "Nenhum usuário encontrado para este aluno"),
                    AssertionConcern.AssertTrue(user.Verified == true, "E-mail não verificado"),
                    AssertionConcern.AssertTrue(user.Active == false, "Cadastro não ativado"),
                    AssertionConcern.AssertAreEquals(user.AuthorizationCode.ToUpper(), authorizationCode.ToUpper(), "Código de autenticação inválido"),
                    AssertionConcern.AssertAreEquals(user.Password, password, "Usuário ou senha inválidos"),
                    AssertionConcern.AssertAreEquals(DateTime.Compare(user.LastAuthorizationCodeRequest.AddMinutes(5), DateTime.Now).ToString(), (1).ToString(), "Código de autenticação expirado")
                 );
        }
    }
}

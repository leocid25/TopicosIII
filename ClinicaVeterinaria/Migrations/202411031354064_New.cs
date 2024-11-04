namespace ClinicaVeterinaria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agendamentoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                        TipoConsulta = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Descricao = c.String(maxLength: 500),
                        NomeCliente = c.String(nullable: false),
                        TelefoneCliente = c.String(nullable: false),
                        VeterinarioId = c.Int(nullable: false),
                        CorFundo = c.String(maxLength: 7),
                        DataCriacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Veterinarios", t => t.VeterinarioId)
                .Index(t => t.VeterinarioId);
            
            CreateTable(
                "dbo.Veterinarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        Telefone = c.String(nullable: false),
                        Cpf = c.String(),
                        Status = c.Int(nullable: false),
                        Endereco = c.String(nullable: false),
                        Especializacao = c.String(),
                        CRMV = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Encontroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        HoraInicio = c.DateTime(nullable: false),
                        HoraTermino = c.DateTime(),
                        TipoConsulta = c.Int(nullable: false),
                        ValorPagoConsulta = c.Double(),
                        Descricao = c.String(),
                        PetId = c.Int(nullable: false),
                        VeterinarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pets", t => t.PetId)
                .ForeignKey("dbo.Veterinarios", t => t.VeterinarioId)
                .Index(t => t.PetId)
                .Index(t => t.VeterinarioId);
            
            CreateTable(
                "dbo.Pets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Especie = c.String(nullable: false),
                        Raca = c.String(),
                        DataNascimento = c.DateTime(nullable: false),
                        Genero = c.String(),
                        ProprietarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proprietarios", t => t.ProprietarioId)
                .Index(t => t.ProprietarioId);
            
            CreateTable(
                "dbo.Proprietarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Cpf = c.String(),
                        Email = c.String(nullable: false),
                        Telefone = c.String(nullable: false),
                        Endereco = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tratamentoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        HoraInicio = c.DateTime(nullable: false),
                        HoraTermino = c.DateTime(),
                        Status = c.Int(),
                        ValorPagoTratamento = c.Double(),
                        PetId = c.Int(nullable: false),
                        EncontroId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Encontroes", t => t.EncontroId)
                .ForeignKey("dbo.Pets", t => t.PetId)
                .Index(t => t.PetId)
                .Index(t => t.EncontroId);
            
            CreateTable(
                "dbo.Receitas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Descricao = c.String(),
                        Dosagem = c.String(),
                        Preco = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Curiosoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Secretarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Cpf = c.String(),
                        Status = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        Telefone = c.String(nullable: false),
                        Endereco = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProprietarioId = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proprietarios", t => t.ProprietarioId)
                .Index(t => t.ProprietarioId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TratamentoReceitas",
                c => new
                    {
                        Tratamento_Id = c.Int(nullable: false),
                        Receita_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tratamento_Id, t.Receita_Id })
                .ForeignKey("dbo.Tratamentoes", t => t.Tratamento_Id, cascadeDelete: true)
                .ForeignKey("dbo.Receitas", t => t.Receita_Id, cascadeDelete: true)
                .Index(t => t.Tratamento_Id)
                .Index(t => t.Receita_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ProprietarioId", "dbo.Proprietarios");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Agendamentoes", "VeterinarioId", "dbo.Veterinarios");
            DropForeignKey("dbo.Encontroes", "VeterinarioId", "dbo.Veterinarios");
            DropForeignKey("dbo.Encontroes", "PetId", "dbo.Pets");
            DropForeignKey("dbo.Tratamentoes", "PetId", "dbo.Pets");
            DropForeignKey("dbo.TratamentoReceitas", "Receita_Id", "dbo.Receitas");
            DropForeignKey("dbo.TratamentoReceitas", "Tratamento_Id", "dbo.Tratamentoes");
            DropForeignKey("dbo.Tratamentoes", "EncontroId", "dbo.Encontroes");
            DropForeignKey("dbo.Pets", "ProprietarioId", "dbo.Proprietarios");
            DropIndex("dbo.TratamentoReceitas", new[] { "Receita_Id" });
            DropIndex("dbo.TratamentoReceitas", new[] { "Tratamento_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "ProprietarioId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Tratamentoes", new[] { "EncontroId" });
            DropIndex("dbo.Tratamentoes", new[] { "PetId" });
            DropIndex("dbo.Pets", new[] { "ProprietarioId" });
            DropIndex("dbo.Encontroes", new[] { "VeterinarioId" });
            DropIndex("dbo.Encontroes", new[] { "PetId" });
            DropIndex("dbo.Agendamentoes", new[] { "VeterinarioId" });
            DropTable("dbo.TratamentoReceitas");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Secretarios");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Curiosoes");
            DropTable("dbo.Receitas");
            DropTable("dbo.Tratamentoes");
            DropTable("dbo.Proprietarios");
            DropTable("dbo.Pets");
            DropTable("dbo.Encontroes");
            DropTable("dbo.Veterinarios");
            DropTable("dbo.Agendamentoes");
        }
    }
}

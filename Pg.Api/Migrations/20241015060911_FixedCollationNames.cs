using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pg.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixedCollationNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:en_ci_as", "en-u-ks-level1,en-u-ks-level1,icu,False")
                .Annotation("Npgsql:CollationDefinition:en_ci_as_like", "en-u-ks-level2,en-u-ks-level2,icu,True")
                .OldAnnotation("Npgsql:CollationDefinition:en_ci_as", "en-u-ks-level1,en-u-ks-level1,icu,False");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:en_ci_as", "en-u-ks-level1,en-u-ks-level1,icu,False")
                .OldAnnotation("Npgsql:CollationDefinition:en_ci_as", "en-u-ks-level1,en-u-ks-level1,icu,False")
                .OldAnnotation("Npgsql:CollationDefinition:en_ci_as_like", "en-u-ks-level2,en-u-ks-level2,icu,True");
        }
    }
}

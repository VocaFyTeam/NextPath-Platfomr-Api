using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VocaFy.NextPath.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddCareerExplorationTasksMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "careers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    emoji = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    area = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    image_url = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    short_description = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    duration_years = table.Column<int>(type: "int", nullable: false),
                    duration_label = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    semesters = table.Column<int>(type: "int", nullable: false),
                    modality = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    modality_detail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    universities = table.Column<string>(type: "json", nullable: false),
                    field_of_work = table.Column<string>(type: "json", nullable: false),
                    main_field = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    skills = table.Column<string>(type: "json", nullable: false),
                    average_salary = table.Column<string>(type: "json", nullable: false),
                    salary_range = table.Column<string>(type: "json", nullable: false),
                    employability_rate = table.Column<int>(type: "int", nullable: false),
                    demand_level = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    base_compatibility = table.Column<int>(type: "int", nullable: false),
                    profile = table.Column<string>(type: "json", nullable: false),
                    projection = table.Column<string>(type: "json", nullable: false),
                    recommendations = table.Column<string>(type: "json", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_careers", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "group_comparison_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    psychologist_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    selected_group_ids = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    aggregated_conclusions = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_group_comparison_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    psychologist_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_student_groups", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student_tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: true),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_student_tasks", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vocational_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    psychologist_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    report_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    content_summary = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false),
                    is_validated_by_psychologist = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    validated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vocational_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "favorite_careers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    career_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_favorite_careers", x => x.id);
                    table.ForeignKey(
                        name: "f_k_favorite_careers_careers_career_id",
                        column: x => x.career_id,
                        principalTable: "careers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "historical_profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    assigned_psychologist_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    group_id = table.Column<int>(type: "int", nullable: true),
                    observation_summary = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: false),
                    flags = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    last_activity_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_historical_profiles", x => x.id);
                    table.ForeignKey(
                        name: "f_k_historical_profiles__student_group_group_id",
                        column: x => x.group_id,
                        principalTable: "student_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "professional_observations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    historical_profile_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    psychologist_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    observation_text = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    agreements = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    session_date = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    digital_signature = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_professional_observations", x => x.id);
                    table.ForeignKey(
                        name: "fk_prof_observations_profile",
                        column: x => x.historical_profile_id,
                        principalTable: "historical_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_careers_area",
                table: "careers",
                column: "area");

            migrationBuilder.CreateIndex(
                name: "i_x_careers_name",
                table: "careers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_favorite_careers_career_id",
                table: "favorite_careers",
                column: "career_id");

            migrationBuilder.CreateIndex(
                name: "i_x_favorite_careers_student_id_career_id",
                table: "favorite_careers",
                columns: new[] { "student_id", "career_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_group_comparison_reports_psychologist_id",
                table: "group_comparison_reports",
                column: "psychologist_id");

            migrationBuilder.CreateIndex(
                name: "i_x_historical_profiles_assigned_psychologist_id",
                table: "historical_profiles",
                column: "assigned_psychologist_id");

            migrationBuilder.CreateIndex(
                name: "i_x_historical_profiles_group_id",
                table: "historical_profiles",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_historical_profiles_student_id",
                table: "historical_profiles",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_professional_observations_historical_profile_id",
                table: "professional_observations",
                column: "historical_profile_id");

            migrationBuilder.CreateIndex(
                name: "i_x_student_groups_psychologist_id_name",
                table: "student_groups",
                columns: new[] { "psychologist_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_student_tasks_student_id_display_order",
                table: "student_tasks",
                columns: new[] { "student_id", "display_order" });

            migrationBuilder.CreateIndex(
                name: "i_x_vocational_reports_psychologist_id",
                table: "vocational_reports",
                column: "psychologist_id");

            migrationBuilder.CreateIndex(
                name: "i_x_vocational_reports_student_id",
                table: "vocational_reports",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorite_careers");

            migrationBuilder.DropTable(
                name: "group_comparison_reports");

            migrationBuilder.DropTable(
                name: "professional_observations");

            migrationBuilder.DropTable(
                name: "student_tasks");

            migrationBuilder.DropTable(
                name: "vocational_reports");

            migrationBuilder.DropTable(
                name: "careers");

            migrationBuilder.DropTable(
                name: "historical_profiles");

            migrationBuilder.DropTable(
                name: "student_groups");
        }
    }
}

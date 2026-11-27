using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VocaFy.NextPath.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "interest_communities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_interest_communities", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    session_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    psychologist_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    scheduled_date_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    meet_link = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    session_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    group_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    grouping_criteria = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    max_participants = table.Column<int>(type: "int", nullable: true),
                    student_id = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k__session", x => x.session_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    is_session_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.user_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vocational_resources",
                columns: table => new
                {
                    resource_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    psychologist_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    cover_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    file_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vocational_resources", x => x.resource_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vocational_tests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    short_name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    headline = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    instructions = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    type = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    featured = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    version = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vocational_tests", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "community_members",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    joined_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    community_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_community_members", x => x.id);
                    table.ForeignKey(
                        name: "f_k_community_members_interest_communities_community_id",
                        column: x => x.community_id,
                        principalTable: "interest_communities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "forum_threads",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    community_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    author_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    author_name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    content = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    comments_count = table.Column<int>(type: "int", nullable: false),
                    likes_count = table.Column<int>(type: "int", nullable: false),
                    is_reported = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    report_reason = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_forum_threads", x => x.id);
                    table.ForeignKey(
                        name: "f_k_forum_threads__interest_community_community_id",
                        column: x => x.community_id,
                        principalTable: "interest_communities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "session_participants",
                columns: table => new
                {
                    student_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    session_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_session_participants", x => new { x.session_id, x.student_id });
                    table.ForeignKey(
                        name: "f_k_session_participants_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "password_reset_tokens",
                columns: table => new
                {
                    token_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    token = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    expiration_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    is_used = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_password_reset_tokens", x => x.token_id);
                    table.ForeignKey(
                        name: "f_k_password_reset_tokens__user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "psychologists",
                columns: table => new
                {
                    psychologist_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    professional_license = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    specialty = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    unique_linking_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_psychologists", x => x.psychologist_id);
                    table.ForeignKey(
                        name: "f_k_psychologists__user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    student_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    school_or_university = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    current_cycle = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    linked_psychologist_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_students", x => x.student_id);
                    table.ForeignKey(
                        name: "f_k_students__user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "test_questions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    vocational_test_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    text = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    area = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    placeholder = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_test_questions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_test_questions_vocational_tests_vocational_test_id",
                        column: x => x.vocational_test_id,
                        principalTable: "vocational_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "test_results",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    student_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    vocational_test_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    version = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    status = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    summary = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    score_artistic = table.Column<int>(type: "int", nullable: false),
                    score_social = table.Column<int>(type: "int", nullable: false),
                    score_investigative = table.Column<int>(type: "int", nullable: false),
                    score_enterprising = table.Column<int>(type: "int", nullable: false),
                    score_conventional = table.Column<int>(type: "int", nullable: false),
                    completed_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_test_results", x => x.id);
                    table.ForeignKey(
                        name: "f_k_test_results__vocational_test_vocational_test_id",
                        column: x => x.vocational_test_id,
                        principalTable: "vocational_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    forum_thread_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    author_name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    content = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_comments", x => x.id);
                    table.ForeignKey(
                        name: "f_k_comments__forum_thread_forum_thread_id",
                        column: x => x.forum_thread_id,
                        principalTable: "forum_threads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "question_options",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    text = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: false),
                    area = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    test_question_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_question_options", x => x.id);
                    table.ForeignKey(
                        name: "f_k_question_options_test_questions_test_question_id",
                        column: x => x.test_question_id,
                        principalTable: "test_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "career_matches",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    career_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    compatibility = table.Column<int>(type: "int", nullable: false),
                    test_result_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_career_matches", x => x.id);
                    table.ForeignKey(
                        name: "f_k_career_matches_test_results_test_result_id",
                        column: x => x.test_result_id,
                        principalTable: "test_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "test_answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    question_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    value = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    test_result_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_test_answers", x => x.id);
                    table.ForeignKey(
                        name: "f_k_test_answers_test_results_test_result_id",
                        column: x => x.test_result_id,
                        principalTable: "test_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_career_matches_test_result_id",
                table: "career_matches",
                column: "test_result_id");

            migrationBuilder.CreateIndex(
                name: "i_x_comments_forum_thread_id",
                table: "comments",
                column: "forum_thread_id");

            migrationBuilder.CreateIndex(
                name: "i_x_community_members_community_id",
                table: "community_members",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "i_x_forum_threads_community_id",
                table: "forum_threads",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "i_x_interest_communities_name",
                table: "interest_communities",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_password_reset_tokens_token",
                table: "password_reset_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_password_reset_tokens_user_id",
                table: "password_reset_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_x_psychologists_unique_linking_code",
                table: "psychologists",
                column: "unique_linking_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_psychologists_user_id",
                table: "psychologists",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_question_options_test_question_id",
                table: "question_options",
                column: "test_question_id");

            migrationBuilder.CreateIndex(
                name: "i_x_session_participants_student_id",
                table: "session_participants",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "i_x__session_psychologist_id",
                table: "sessions",
                column: "psychologist_id");

            migrationBuilder.CreateIndex(
                name: "i_x__session_scheduled_date_time",
                table: "sessions",
                column: "scheduled_date_time");

            migrationBuilder.CreateIndex(
                name: "i_x_sessions_student_id",
                table: "sessions",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "i_x_students_user_id",
                table: "students",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_test_answers_test_result_id",
                table: "test_answers",
                column: "test_result_id");

            migrationBuilder.CreateIndex(
                name: "i_x_test_questions_vocational_test_id",
                table: "test_questions",
                column: "vocational_test_id");

            migrationBuilder.CreateIndex(
                name: "i_x_test_results_vocational_test_id",
                table: "test_results",
                column: "vocational_test_id");

            migrationBuilder.CreateIndex(
                name: "i_x_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_vocational_resources_psychologist_id",
                table: "vocational_resources",
                column: "psychologist_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "career_matches");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "community_members");

            migrationBuilder.DropTable(
                name: "password_reset_tokens");

            migrationBuilder.DropTable(
                name: "psychologists");

            migrationBuilder.DropTable(
                name: "question_options");

            migrationBuilder.DropTable(
                name: "session_participants");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "test_answers");

            migrationBuilder.DropTable(
                name: "vocational_resources");

            migrationBuilder.DropTable(
                name: "forum_threads");

            migrationBuilder.DropTable(
                name: "test_questions");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "test_results");

            migrationBuilder.DropTable(
                name: "interest_communities");

            migrationBuilder.DropTable(
                name: "vocational_tests");
        }
    }
}

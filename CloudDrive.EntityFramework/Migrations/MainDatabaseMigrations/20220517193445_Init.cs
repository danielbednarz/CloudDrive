﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudDrive.EntityFramework.Migrations.MainDatabaseMigrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDirectories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ParentDirectoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDirectories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDirectories_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDirectories_UserDirectories_ParentDirectoryId",
                        column: x => x.ParentDirectoryId,
                        principalTable: "UserDirectories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    FileVersion = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DirectoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_UserDirectories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "UserDirectories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FileOperationsLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileOperationsLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileOperationsLogs_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileOperationsLogs_FileId",
                table: "FileOperationsLogs",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_DirectoryId",
                table: "Files",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserId",
                table: "Files",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDirectories_ParentDirectoryId",
                table: "UserDirectories",
                column: "ParentDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDirectories_UserId",
                table: "UserDirectories",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileOperationsLogs");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "UserDirectories");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}

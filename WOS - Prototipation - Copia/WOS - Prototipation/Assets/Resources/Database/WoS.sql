-- MySQL Script generated by MySQL Workbench
-- Ter 19 Jun 2018 17:29:39 -03
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Album`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Album` (
  `idAlbum` INT NOT NULL,
  `ab_1` VARCHAR(500) NULL,
  `ab_2` VARCHAR(500) NULL,
  `ab_3` VARCHAR(500) NULL,
  `ab_4` VARCHAR(500) NULL,
  `ab_5` VARCHAR(500) NULL,
  PRIMARY KEY (`idAlbum`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Usuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Usuario` (
  `idUsuario` INT NOT NULL,
  `nome` VARCHAR(50) NOT NULL,
  `email` VARCHAR(50) NOT NULL,
  `senha` VARCHAR(200) NOT NULL,
  `Album_idAlbum` INT NOT NULL,
  PRIMARY KEY (`idUsuario`),
  INDEX `fk_Usuario_Album1_idx` (`Album_idAlbum` ASC))
ENGINE = MyISAM;


-- -----------------------------------------------------
-- Table `mydb`.`Era`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Era` (
  `idEra` INT NOT NULL,
  `descricaoEra` VARCHAR(50) NOT NULL,
  `idadeEra` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idEra`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Cartas`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Cartas` (
  `idCartas` INT NOT NULL,
  `nome` VARCHAR(50) NOT NULL,
  `descricaoCarta` TEXT NOT NULL,
  `dataCarta` VARCHAR(50) NULL,
  `Era_idEra` INT NOT NULL,
  PRIMARY KEY (`idCartas`),
  INDEX `fk_Cartas_Era_idx` (`Era_idEra` ASC),
  CONSTRAINT `fk_Cartas_Era`
    FOREIGN KEY (`Era_idEra`)
    REFERENCES `mydb`.`Era` (`idEra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`InforEstatica`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`InforEstatica` (
  `introducao` TEXT NOT NULL,
  `equipe` TEXT NOT NULL,
  `descricaoUsoPerfil` TEXT NOT NULL)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

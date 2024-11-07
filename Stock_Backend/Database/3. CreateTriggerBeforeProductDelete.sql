DROP TRIGGER IF EXISTS BeforeProductDelete;

CREATE TRIGGER BeforeProductDelete

BEFORE DELETE ON Product

FOR EACH ROW
BEGIN
    -- Verificar se o produto possui registros na tabela Inventory
    IF EXISTS (SELECT 1 FROM Inventory WHERE ProductId = OLD.Id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Não é possível excluir o produto. O produto está relacionado com o inventário.';
    END IF;

    -- Verificar se o produto possui registros na tabela InventoryMovement
    IF EXISTS (SELECT 1 FROM InventoryMovement WHERE ProductId = OLD.Id) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Não é possível excluir o produto. O produto está relacionado com o movimento de inventário.';
    END IF;
END;

DROP PROCEDURE IF EXISTS UpdateInventoryAndLogMovement;

CREATE PROCEDURE UpdateInventoryAndLogMovement(
    IN p_productId INT,
    IN p_quantity INT,
    IN p_movementType CHAR(1),  -- "I" or "O"
    OUT p_message VARCHAR(255)  -- mensagem de resultado
)
proc_end: BEGIN
    DECLARE v_availableQuantity INT DEFAULT 0;
    DECLARE v_averagePriceCost DECIMAL(10, 2) DEFAULT 0.00;

    -- Verifica se o produto existe
    IF EXISTS (SELECT 1 FROM Product WHERE Id = p_productId) THEN
        
        -- Obtém o preço médio do custo do produto
        SELECT AverageCostPrice INTO v_averagePriceCost
        FROM Product 
        WHERE Id = p_productId;

        -- Verifica se já existe um registro do produto na tabela "Inventory"
        IF EXISTS (SELECT 1 FROM Inventory WHERE ProductId = p_productId) THEN
            -- Obtem a quantidade atual do estoque
            SELECT AvailableQuantity INTO v_availableQuantity 
            FROM Inventory 
            WHERE ProductId = p_productId;

            -- Se for um movimento de saída e houver quantidade o suficiente no estoque
            IF p_movementType = 'O' THEN
                IF v_availableQuantity >= p_quantity THEN
                    -- Atualiza a quantidade
                    UPDATE Inventory
                    SET AvailableQuantity = AvailableQuantity - p_quantity
                    WHERE ProductId = p_productId;
                    SET p_message = 'Sucesso';
                ELSE
                    SET p_message = 'Quantidade insuficiente no estoque!';
                    LEAVE proc_end;
                END IF;
            ELSE
                -- Se for uma movimentação de entrada
                UPDATE Inventory
                SET AvailableQuantity = AvailableQuantity + p_quantity
                WHERE ProductId = p_productId;
                SET p_message = 'Sucesso';
            END IF;
        
        ELSE
            -- Se ainda não existir um registro do produto no estoque
            INSERT INTO Inventory (ProductId, AvailableQuantity)
            VALUES (p_productId, p_quantity);
            SET p_message = 'Sucesso';
        END IF;

        -- Registra a movimentação usando o valor médio de custo obtido
        INSERT INTO InventoryMovement (ProductId, MovementType, Quantity, AverageCost, MovementDate)
        VALUES (p_productId, p_movementType, p_quantity, v_averagePriceCost, NOW());

    ELSE
        SET p_message = 'Produto não localizado!';
    END IF;

END;

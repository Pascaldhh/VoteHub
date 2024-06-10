describe('Create poll', () => {
    it('passes', () => {
        cy.visit('http://localhost:5066/Identity/Account/Login')
        cy.get('[name="Input.Email"]')
            .type('admin@admin.com')
        cy.get('[name="Input.Password"]')
            .type("Admin12345!")
        cy.get("#login-submit").click();

        cy.url().should("equal", "http://localhost:5066/")

        cy.visit("http://localhost:5066/Admin");
        cy.get("#create-poll").click();

        cy.get("#Question").type("My test question 1!")
        cy.get('#Options').type("This is the first answer, This is the second answer, This is the third answer")
        cy.get('#create-edit').click();

        cy.get("table tr:last-child td:first-child").should("contain.text", "My test question 1!")
    })
})
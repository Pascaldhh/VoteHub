describe('Login', () => {
  it('passes', () => {
    cy.visit('http://localhost:5066/Identity/Account/Login')
    cy.get('[name="Input.Email"]')
        .type('voter@voter.com')
    cy.get('[name="Input.Password"]')
        .type("Voter12345!")
    cy.get("#login-submit").click();

    cy.url().should("equal", "http://localhost:5066/")
  })
})

describe('Register', () => {
    it('passes', () => {
        const random = Math.random();

        cy.visit('http://localhost:5066/Identity/Account/Register')
        cy.get('[name="Input.Email"]')
            .type(`${random}test@test.com`)
        cy.get('[name="Input.Password"]')
            .type("Test12345!")
        cy.get('[name="Input.ConfirmPassword"]')
            .type("Test12345!")
        cy.get("#registerSubmit").click();

        cy.url().should("include", "RegisterConfirmation");
    })
})
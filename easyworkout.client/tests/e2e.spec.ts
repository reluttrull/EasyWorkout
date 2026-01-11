import { test, expect } from '@playwright/test';

test('e2e flow 1', async ({ page, isMobile }) => {
    await page.goto('/EasyWorkout/register');
    await page.getByRole('textbox', { name: 'Username' }).click();
    await page.getByRole('textbox', { name: 'Username' }).fill('Playwright');
    await page.getByRole('textbox', { name: 'Username' }).press('Tab');
    await page.getByRole('textbox', { name: 'Email' }).fill('email@email.com');
    await page.getByRole('textbox', { name: 'Email' }).press('Tab');
    await page.getByRole('textbox', { name: 'First name' }).press('Tab');
    await page.getByRole('textbox', { name: 'Last name' }).press('Tab');
    await page.getByRole('textbox', { name: 'Password', exact: true }).fill('Playwright1!');
    await page.getByRole('textbox', { name: 'Password', exact: true }).press('Tab');
    await page.getByRole('textbox', { name: 'Confirm password' }).fill('Playwright1!');
    await page.getByRole('button', { name: 'Register' }).click();
    await page.waitForURL('/EasyWorkout/login');
    await page.getByRole('textbox', { name: 'Username' }).click();
    await page.getByRole('textbox', { name: 'Username' }).fill('Playwright');
    await page.getByRole('textbox', { name: 'Username' }).press('Tab');
    await page.getByRole('textbox', { name: 'Password' }).fill('Playwright1!');
    await page.getByRole('button', { name: 'Login' }).click();
    await page.waitForURL('/EasyWorkout/workouts');
    await page.getByRole('button', { name: 'Create new workout' }).click();
    await page.getByRole('textbox', { name: 'Name' }).click();
    await page.getByRole('textbox', { name: 'Name' }).fill('Test Workout');
    await page.getByRole('textbox', { name: 'Name' }).press('Tab');
    await page.getByRole('button', { name: 'Save workout' }).click();
    await page.getByRole('button', { name: 'show exercise detail' }).click();
    await page.getByRole('button', { name: 'Add exercise' }).click();
    await page.getByRole('button', { name: 'Create new exercise' }).click();
    await page.getByRole('textbox', { name: 'Name' }).click();
    await page.getByRole('textbox', { name: 'Name' }).fill('Test Exercise');
    await page.getByRole('button', { name: 'Save exercise' }).click();
    await page.getByRole('button', { name: 'show set detail' }).click();
    await page.getByRole('button', { name: 'Add set' }).click();
    // await page.locator('div').filter({ hasText: /^Reps$/ }).nth(3).click();
    await page.getByText('Reps').click();
    await page.getByRole('spinbutton', { name: 'Reps' }).fill('5');
    await page.getByText('Weight', { exact: true }).click();
    await page.getByRole('spinbutton', { name: 'Weight' }).fill('30');
    await page.getByRole('combobox', { name: 'Weight units:' }).locator('path').click();
    await page.getByRole('option', { name: 'Pounds' }).click();
    await page.getByRole('button', { name: 'Save set' }).click();
    await page.getByRole('button', { name: 'menu for workout-related' }).click();
    await page.getByRole('menuitem', { name: 'Start workout' }).click();
    await page.waitForURL('/EasyWorkout/do-workout**');
    await page.getByText('Reps').click();
    await page.getByRole('spinbutton', { name: 'Reps' }).fill('4');
    await page.getByText('Weight').click();
    await page.getByRole('spinbutton', { name: 'Weight' }).fill('30');
    await page.getByRole('main').click();
    await expect(page.getByRole('main')).toMatchAriaSnapshot(`
      - main:
        - 'heading "Doing workout: Test Workout" [level=2]'
        - 'heading "Exercise 1: Test Exercise" [level=3]'
        - button "Remove Exercise"
        - strong: "Set 1:"
        - text: Reps
        - spinbutton "Reps"
        - text: / 5 Weight
        - spinbutton "Weight"
        - text: / 30Pounds
        - button "remove set"
        - button "add empty set"
        - separator
        - text: New exercise name
        - textbox "New exercise name"
        - button "Add Exercise"
        - separator
        - button "Submit"
      `);
    await page.getByRole('button', { name: 'Submit' }).click();
    await page.waitForURL('/EasyWorkout/completed**');
    if (isMobile) {
      await page.getByLabel('navigation menu').click();
      await page.getByRole('menuitem', { name: 'Account info' }).click();
    } else {
      await page.getByRole('link', { name: 'Account info' }).click();
    }
    page.waitForURL('/EasyWorkout/account');
    await page.getByRole('button', { name: 'Delete account' }).click();
    page.once('dialog', dialog => {
      console.log(`Dialog message: ${dialog.message()}`);
      dialog.accept().catch(() => {});
    });
    await page.getByRole('button', { name: 'Delete my account' }).click();
});
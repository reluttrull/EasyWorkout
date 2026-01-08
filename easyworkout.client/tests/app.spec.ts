import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  await page.goto('');

  // expect title to contain EasyWorkout
  await expect(page).toHaveTitle(/EasyWorkout/);
});

test('has nav menu elements', async ({ page }) => {
  await page.goto('');

  // expect logo visible
  await expect(page.locator('.main-logo')).toBeVisible();
});
import { GreetingPipe } from './greeting-pipe';

describe('GreetingPipe', () => {
  it('create an instance', () => {
    const pipe = new GreetingPipe();
    expect(pipe).toBeTruthy();
  });
});

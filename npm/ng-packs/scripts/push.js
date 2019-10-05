const Confirm = require('prompt-confirm');
const execa = require('execa');

(async () => {
  const answer = await new Confirm('Would you like to push?').run().then(answer => answer);

  if (answer) {
    try {
      await execa('git', ['push'], { stdout: 'inherit' });
      console.log('Successfully!');
    } catch (error) {
      console.log('An error occured.' + error);
    }
  }
})();

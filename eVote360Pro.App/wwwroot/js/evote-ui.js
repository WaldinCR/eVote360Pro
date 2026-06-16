// eVote360 Pro — UI Interactions
document.addEventListener('DOMContentLoaded', function () {

    // ── OTP auto-advance ──
    const otpBoxes = document.querySelectorAll('.ob');
    otpBoxes.forEach((box, i) => {
        box.addEventListener('input', () => {
            if (box.value && i < otpBoxes.length - 1) otpBoxes[i + 1].focus();
        });
        box.addEventListener('keydown', e => {
            if (e.key === 'Backspace' && !box.value && i > 0) otpBoxes[i - 1].focus();
        });
    });

    // ── Sync OTP hidden input before form submit ──
    const otpForm = document.getElementById('otp-form');
    if (otpForm) {
        otpForm.addEventListener('submit', function () {
            const digits = Array.from(otpBoxes).map(b => b.value).join('');
            const hidden = document.getElementById('otp-hidden');
            if (hidden) hidden.value = digits;
        });
    }

    // ── OTP Countdown timer ──
    const timerEl = document.getElementById('otp-timer');
    if (timerEl) {
        let secs = parseInt(timerEl.dataset.seconds || '299');
        function tick() {
            const m = Math.floor(secs / 60), s = secs % 60;
            timerEl.textContent = (m < 10 ? '0' : '') + m + ':' + (s < 10 ? '0' : '') + s;
            if (secs > 0) { secs--; setTimeout(tick, 1000); }
            else { timerEl.style.color = 'var(--r800)'; }
        }
        tick();
    }

    // ── Candidate card selection (ballot) ──
    document.querySelectorAll('.ccard2').forEach(card => {
        card.addEventListener('click', function () {
            const grid = this.closest('.bgrid');
            if (!grid) return;
            grid.querySelectorAll('.ccard2').forEach(c => c.classList.remove('sel2'));
            this.classList.add('sel2');
            const radio = this.querySelector('input[type="radio"]');
            if (radio) radio.checked = true;
        });
    });

    // ── Upload zone click triggers file input ──
    document.querySelectorAll('.upz').forEach(zone => {
        zone.addEventListener('click', function () {
            const fileInput = this.querySelector('input[type="file"]');
            if (fileInput) fileInput.click();
        });
    });

    // ── Auto-dismiss notifications after 4 seconds ──
    document.querySelectorAll('.evote-notif').forEach(notif => {
        setTimeout(() => {
            notif.style.transition = 'opacity .4s, transform .4s';
            notif.style.opacity = '0';
            notif.style.transform = 'translateY(-10px)';
            setTimeout(() => notif.remove(), 400);
        }, 4000);
    });
});

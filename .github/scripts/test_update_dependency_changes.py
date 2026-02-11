#!/usr/bin/env python3
"""
Comprehensive test suite for update_dependency_changes.py

Tests cover:
- Basic update/add/remove scenarios
- Version revert scenarios
- Complex multi-step change sequences
- Edge cases and duplicate operations
- Document format validation
"""

import sys
import os
sys.path.insert(0, os.path.dirname(__file__))

from update_dependency_changes import merge_changes, render_section


def test_update_then_revert():
    """Test: PR1 updates A->B, PR2 reverts B->A. Should be removed."""
    print("Test 1: Update then revert")
    existing = (
        {"PackageA": ("1.0.0", "2.0.0", "#1")},  # updated
        {},  # added
        {}   # removed
    )
    new = (
        {"PackageA": ("2.0.0", "1.0.0", "#2")},  # updated back
        {},
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageA" not in updated, f"Expected PackageA removed, got: {updated}"
    assert len(added) == 0 and len(removed) == 0
    print("✓ Passed: Package correctly removed from updates\n")


def test_add_then_remove_same_version():
    """Test: PR1 adds v1.0, PR2 removes v1.0. Should be completely removed."""
    print("Test 2: Add then remove same version")
    existing = (
        {},
        {"PackageB": ("1.0.0", "#1")},  # added
        {}
    )
    new = (
        {},
        {},
        {"PackageB": ("1.0.0", "#2")}  # removed
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageB" not in added, f"Expected PackageB removed from added, got: {added}"
    assert "PackageB" not in removed, f"Expected PackageB removed from removed, got: {removed}"
    assert "PackageB" not in updated
    print("✓ Passed: Package correctly removed from all sections\n")


def test_remove_then_add_same_version():
    """Test: PR1 removes v1.0, PR2 adds v1.0. Should be removed."""
    print("Test 3: Remove then add same version")
    existing = (
        {},
        {},
        {"PackageC": ("1.0.0", "#1")}  # removed
    )
    new = (
        {},
        {"PackageC": ("1.0.0", "#2")},  # added back
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageC" not in updated, f"Expected PackageC removed from updated, got: {updated}"
    assert "PackageC" not in added, f"Expected PackageC removed from added, got: {added}"
    assert "PackageC" not in removed, f"Expected PackageC removed from removed, got: {removed}"
    print("✓ Passed: Package correctly removed from all sections\n")


def test_add_then_remove_different_version():
    """Test: PR1 adds v1.0, PR2 removes v2.0. Should show as removed v2.0."""
    print("Test 4: Add then remove different version")
    existing = (
        {},
        {"PackageD": ("1.0.0", "#1")},  # added
        {}
    )
    new = (
        {},
        {},
        {"PackageD": ("2.0.0", "#2")}  # removed different version
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageD" not in added, f"Expected PackageD removed from added, got: {added}"
    assert "PackageD" in removed, f"Expected PackageD in removed, got: {removed}"
    assert removed["PackageD"][0] == "2.0.0", f"Expected version 2.0.0, got: {removed['PackageD']}"
    print(f"✓ Passed: Package correctly tracked as removed with version {removed['PackageD'][0]}\n")


def test_update_in_added():
    """Test: PR1 adds v1.0, PR2 updates to v2.0. Should show as updated 1.0->2.0."""
    print("Test 5: Update a package that was added")
    existing = (
        {},
        {"PackageE": ("1.0.0", "#1")},  # added
        {}
    )
    new = (
        {"PackageE": ("1.0.0", "2.0.0", "#2")},  # updated
        {},
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageE" not in added, f"Expected PackageE removed from added, got: {added}"
    assert "PackageE" in updated, f"Expected PackageE in updated, got: {updated}"
    assert updated["PackageE"] == ("1.0.0", "2.0.0", "#1, #2"), \
        f"Expected ('1.0.0', '2.0.0', '#1, #2'), got: {updated['PackageE']}"
    print(f"✓ Passed: Package correctly converted to updated: {updated['PackageE']}\n")


def test_multiple_updates():
    """Test: PR1 updates A->B, PR2 updates B->C. Should show A->C."""
    print("Test 6: Multiple updates")
    existing = (
        {"PackageF": ("1.0.0", "2.0.0", "#1")},  # updated
        {},
        {}
    )
    new = (
        {"PackageF": ("2.0.0", "3.0.0", "#2")},  # updated again
        {},
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageF" in updated
    assert updated["PackageF"] == ("1.0.0", "3.0.0", "#1, #2"), \
        f"Expected ('1.0.0', '3.0.0', '#1, #2'), got: {updated['PackageF']}"
    print(f"✓ Passed: Package correctly shows full range: {updated['PackageF']}\n")


def test_multiple_updates_back_to_original():
    """Test: PR1 updates 1->2, PR2 updates 2->3, PR3 updates 3->1. Should be removed."""
    print("Test 7: Multiple updates ending back at original version")
    # Simulate PR1 and PR2 already merged
    existing = (
        {"PackageG": ("1.0.0", "3.0.0", "#1, #2")},  # updated through PR1 and PR2
        {},
        {}
    )
    # PR3 changes back to 1.0.0
    new = (
        {"PackageG": ("3.0.0", "1.0.0", "#3")},  # updated back to original
        {},
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageG" not in updated, f"Expected PackageG removed, got: {updated}"
    assert len(added) == 0 and len(removed) == 0
    print("✓ Passed: Package correctly removed (version returned to original)\n")


def test_update_remove_add_same_version():
    """Test: PR1 updates 1->2, PR2 updates 2->3, PR3 removes, PR4 adds v3. Should show updated 1->3."""
    print("Test 8: Update-Update-Remove-Add same version")
    # After PR1, PR2, PR3
    existing = (
        {},
        {},
        {"PackageH": ("1.0.0", "#1, #2, #3")}  # removed (original was 1.0.0)
    )
    # PR4 adds back the same version that was removed
    new = (
        {},
        {"PackageH": ("3.0.0", "#4")},  # added
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageH" in updated, f"Expected PackageH in updated, got: updated={updated}, added={added}, removed={removed}"
    assert updated["PackageH"] == ("1.0.0", "3.0.0", "#1, #2, #3, #4"), \
        f"Expected ('1.0.0', '3.0.0', '#1, #2, #3, #4'), got: {updated['PackageH']}"
    print(f"✓ Passed: Package correctly shows as updated: {updated['PackageH']}\n")


def test_update_remove_add_original_version():
    """Test: PR1 updates 1->2, PR2 updates 2->3, PR3 removes, PR4 adds v1. Should be removed."""
    print("Test 9: Update-Update-Remove-Add original version")
    # After PR1, PR2, PR3
    existing = (
        {},
        {},
        {"PackageI": ("1.0.0", "#1, #2, #3")}  # removed (original was 1.0.0)
    )
    # PR4 adds back the original version
    new = (
        {},
        {"PackageI": ("1.0.0", "#4")},  # added back to original
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageI" not in updated, f"Expected PackageI removed, got: updated={updated}"
    assert "PackageI" not in added, f"Expected PackageI removed, got: added={added}"
    assert "PackageI" not in removed, f"Expected PackageI removed, got: removed={removed}"
    print("✓ Passed: Package correctly removed (added back to original version)\n")


def test_update_remove_add_different_version():
    """Test: PR1 updates 1->2, PR2 updates 2->3, PR3 removes, PR4 adds v4. Should show updated 1->4."""
    print("Test 10: Update-Update-Remove-Add different version")
    # After PR1, PR2, PR3
    existing = (
        {},
        {},
        {"PackageJ": ("1.0.0", "#1, #2, #3")}  # removed (original was 1.0.0)
    )
    # PR4 adds a completely different version
    new = (
        {},
        {"PackageJ": ("4.0.0", "#4")},  # added new version
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageJ" in updated, f"Expected PackageJ in updated, got: updated={updated}, added={added}, removed={removed}"
    assert updated["PackageJ"] == ("1.0.0", "4.0.0", "#1, #2, #3, #4"), \
        f"Expected ('1.0.0', '4.0.0', '#1, #2, #3, #4'), got: {updated['PackageJ']}"
    print(f"✓ Passed: Package correctly shows as updated: {updated['PackageJ']}\n")


def test_add_update_remove():
    """Test: PR1 adds v1, PR2 updates to v2, PR3 removes v2. Should be completely removed."""
    print("Test 11: Add-Update-Remove")
    # After PR1 and PR2
    existing = (
        {"PackageK": ("1.0.0", "2.0.0", "#1, #2")},  # updated (was added in PR1, updated in PR2)
        {},
        {}
    )
    # PR3 removes v2
    new = (
        {},
        {},
        {"PackageK": ("2.0.0", "#3")}  # removed
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageK" not in updated, f"Expected PackageK removed from updated, got: {updated}"
    assert "PackageK" not in added, f"Expected PackageK removed from added, got: {added}"
    assert "PackageK" in removed, f"Expected PackageK in removed, got: {removed}"
    # The removed should track from the original first version
    assert removed["PackageK"][0] == "1.0.0", f"Expected removed from 1.0.0, got: {removed['PackageK']}"
    print(f"✓ Passed: Package correctly shows as removed from original: {removed['PackageK']}\n")


def test_add_remove_add_same_version():
    """Test: PR1 adds v1, PR2 removes v1, PR3 adds v1 again. Should show as added v1."""
    print("Test 12: Add-Remove-Add same version")
    # After PR1 and PR2 (added then removed)
    existing = (
        {},
        {},
        {}  # Completely removed after PR2
    )
    # PR3 adds v1 again
    new = (
        {},
        {"PackageL": ("1.0.0", "#3")},  # added
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageL" in added, f"Expected PackageL in added, got: added={added}"
    assert added["PackageL"] == ("1.0.0", "#3"), f"Expected ('1.0.0', '#3'), got: {added['PackageL']}"
    print(f"✓ Passed: Package correctly shows as added: {added['PackageL']}\n")


def test_update_remove_remove():
    """Test: PR1 updates 1->2, PR2 removes v2, PR3 tries to remove again. Should show removed from v1."""
    print("Test 13: Update-Remove (duplicate remove)")
    # After PR1 and PR2
    existing = (
        {},
        {},
        {"PackageM": ("1.0.0", "#1, #2")}  # removed (original was 1.0.0)
    )
    # PR3 tries to remove again (edge case, might not happen in practice)
    new = (
        {},
        {},
        {"PackageM": ("1.0.0", "#3")}  # removed again
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageM" in removed, f"Expected PackageM in removed, got: {removed}"
    # Should keep the original information
    assert removed["PackageM"][0] == "1.0.0", f"Expected removed from 1.0.0, got: {removed['PackageM']}"
    print(f"✓ Passed: Package correctly maintains removed state: {removed['PackageM']}\n")


def test_add_add():
    """Test: PR1 adds v1, PR2 adds v2 (version changed externally). Should show added v2."""
    print("Test 14: Add-Add (version changed between PRs)")
    # After PR1
    existing = (
        {},
        {"PackageN": ("1.0.0", "#1")},  # added
        {}
    )
    # PR2 adds different version (edge case)
    new = (
        {},
        {"PackageN": ("2.0.0", "#2")},  # added different version
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageN" in added, f"Expected PackageN in added, got: {added}"
    assert added["PackageN"][0] == "2.0.0", f"Expected version 2.0.0, got: {added['PackageN']}"
    print(f"✓ Passed: Package correctly shows latest added version: {added['PackageN']}\n")


def test_complex_chain_ending_in_original():
    """Test: Complex chain - Add v1, Update to v2, Remove, Add v2, Update to v1. Should be removed."""
    print("Test 15: Complex chain ending at nothing changed")
    # After PR1 (add), PR2 (update), PR3 (remove), PR4 (add back)
    existing = (
        {"PackageO": ("1.0.0", "2.0.0", "#1, #2, #3, #4")},  # Complex history
        {},
        {}
    )
    # PR5 updates back to v1 (original from perspective of first state)
    new = (
        {"PackageO": ("2.0.0", "1.0.0", "#5")},  # back to start
        {},
        {}
    )
    updated, added, removed = merge_changes(existing, new)
    assert "PackageO" not in updated, f"Expected PackageO removed, got: {updated}"
    print(f"✓ Passed: Complex chain correctly removed when ending at original\n")


def test_document_format():
    """Test: Verify the document rendering format."""
    print("Test 16: Document format validation")
    
    updated = {
        "Microsoft.Extensions.Logging": ("8.0.0", "8.0.1", "#123"),
        "Newtonsoft.Json": ("13.0.1", "13.0.3", "#456, #789"),
    }
    
    added = {
        "Azure.Identity": ("1.10.0", "#567"),
    }
    
    removed = {
        "System.Text.Json": ("7.0.0", "#890"),
    }
    
    document = render_section("9.0.0", updated, added, removed)
    
    # Verify document structure
    assert "## 9.0.0" in document, "Version header missing"
    assert "| Package | Old Version | New Version | PR |" in document, "Updated table header missing"
    assert "Microsoft.Extensions.Logging" in document, "Updated package missing"
    assert "**Added:**" in document, "Added section missing"
    assert "Azure.Identity" in document, "Added package missing"
    assert "**Removed:**" in document, "Removed section missing"
    assert "System.Text.Json" in document, "Removed package missing"
    
    print("✓ Passed: Document format is correct")
    print("\nSample output:")
    print("-" * 60)
    print(document)
    print("-" * 60 + "\n")


def run_all_tests():
    """Run all test cases."""
    print("=" * 70)
    print("Testing update_dependency_changes.py")
    print("=" * 70 + "\n")
    
    test_update_then_revert()
    test_add_then_remove_same_version()
    test_remove_then_add_same_version()
    test_add_then_remove_different_version()
    test_update_in_added()
    test_multiple_updates()
    test_multiple_updates_back_to_original()
    test_update_remove_add_same_version()
    test_update_remove_add_original_version()
    test_update_remove_add_different_version()
    test_add_update_remove()
    test_add_remove_add_same_version()
    test_update_remove_remove()
    test_add_add()
    test_complex_chain_ending_in_original()
    test_document_format()
    
    print("=" * 70)
    print("All 16 tests passed! ✓")
    print("=" * 70)
    print("\nTest coverage summary:")
    print("  ✓ Basic scenarios (update, add, remove)")
    print("  ✓ Version revert handling")
    print("  ✓ Complex multi-step sequences")
    print("  ✓ Edge cases and duplicates")
    print("  ✓ Document format validation")
    print("=" * 70)


if __name__ == "__main__":
    run_all_tests()
